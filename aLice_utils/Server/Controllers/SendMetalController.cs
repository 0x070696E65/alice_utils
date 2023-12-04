using System.Text.Json;
using aLice_utils.Client.Services;
using CatSdk.Facade;
using CatSdk.Symbol;
using CatSdk.Utils;
using Microsoft.AspNetCore.Mvc;

namespace aLice_utils.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class SendMetalController : ControllerBase
{
    [HttpGet]
    public string Get([FromQuery] string type)
    {
        return type switch
        {
            "websocket_url" => Environment.GetEnvironmentVariable("WEBSOCKET_URL") ?? throw new InvalidOperationException(),
            "host_url" => Environment.GetEnvironmentVariable("HOST_URL") ?? throw new InvalidOperationException(),
            _ => throw new Exception("type is not correct format")
        };
    }

    [HttpPost]
    public async Task<string> Post([FromBody] Dictionary<string, string> data)
    {
        try
        {
            if (data == null) throw new Exception("data is not correct format");
            if (!data.ContainsKey("uid")) throw new Exception("uid is nothing");
            if (!data.ContainsKey("node")) throw new Exception("node is nothing");
            if (!data.ContainsKey("pubkey")) throw new Exception("pubkey is nothing");
            
            var uid = data["uid"];
            var node = Converter.HexToUtf8(data["node"]);
            var counter = 0;
            
            var webSocketService = new WebSocketService();
            await webSocketService.ConnectAsync("wss://alice-ws.fly.dev");

            var hashes = new List<string>();
            while (true)
            {
                try
                {
                    var signed = data[$"signed{counter}"];
                    counter++;
                    
                    var tx = TransactionFactory.Deserialize(signed);
                    var network = tx.Network == NetworkType.MAINNET ? Network.MainNet : Network.TestNet;
                    var facade = new SymbolFacade(network);
                    var hash = facade.HashTransaction(tx);
                    hashes.Add(Converter.BytesToHex(hash.bytes));
                    /*var messageData = new { targetId = uid, message = Converter.BytesToHex(hash.bytes)};
                
                    await webSocketService.SendAsync(messageData);*/
                    await NodeServices.Announce(node, signed);
                }
                catch
                {
                    break;
                }
            }
            
            var messageData = new { targetId = uid, message = string.Join("_", hashes)};
            await webSocketService.SendAsync(messageData);

            var options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            var dic = new Dictionary<string, string>()
            {
                {"uid", uid},
                {"hashes", JsonSerializer.Serialize(hashes, options)}
            };
            var json = JsonSerializer.Serialize(dic, options);
            json = json.Replace("\\\"", "\""); // ダブルクォーテーションのエスケープを解除
            return json;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            return e.Message;
        }
    }
}