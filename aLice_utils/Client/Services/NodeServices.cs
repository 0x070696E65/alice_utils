using System.Text;

namespace aLice_utils.Client.Services;

public class NodeServices
{
    public async Task<string> GetNode(string networkType)
    {
        if (networkType == "MainNet")
        {
            while (true)
            {
                try
                {
                    var http = new HttpClient();
                    var r = new Random().Next(MainNetNodes.Count - 1);
                    var node = MainNetNodes[r];
                    var res = await (await http.GetAsync($"{node}/node/health")).Content.ReadAsStringAsync();
                    var nodeHealth = System.Text.Json.JsonSerializer.Deserialize<NodeHealth>(res);
                    if (nodeHealth is {status: {apiNode: "up",db: "up"} })
                    {
                        return node;   
                    }
                    else
                    {
                        Console.WriteLine("Node is down...");
                        Console.WriteLine(node);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
        else
        {
            while (true)
            {
                try
                {
                    var http = new HttpClient();
                    var r = new Random().Next(TestNetNodes.Count - 1);
                    var node = TestNetNodes[r];
                    var res = await (await http.GetAsync($"{node}/node/health")).Content.ReadAsStringAsync();
                    var nodeHealth = System.Text.Json.JsonSerializer.Deserialize<NodeHealth>(res);
                    if (nodeHealth is {status: {apiNode: "up",db: "up"} })
                    {
                        return node;
                    }
                    else
                    {
                        Console.WriteLine("Node is down...");
                        Console.WriteLine(node);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
    }
    
    private readonly List<string> TestNetNodes = new List<string> { "https://001-sai-dual.symboltest.net:3001", "https://vmi831828.contaboserver.net:3001", "https://mikun-testnet.tk:3001", "https://sym-test-01.opening-line.jp:3001" };
    private readonly List<string> MainNetNodes = new List<string> { "https://wolf.importance.jp:3001", "https://symbol-node.harvest-xym.com:3001", "https://xxx-welcome-to-a-powerful-node.com:3001", "https://symbol.pan-farm.com:3001", "https://00A06705.xym.stir-hosyu.com:3001", "https://00fabf14.xym.stir-hosyu.com:3001", "https://xym10.allnodes.me:3001", "https://xym767.allnodes.me:3001", "https://xym704.allnodes.me:3001", "https://xym686.allnodes.me:3001", "https://xym660.allnodes.me:3001" };

    public class NodeHealth
    {
        public NodeStatus? status { get; set; }
    }
    public class NodeStatus
    {
        public string? apiNode { get; set; }
        public string? db { get; set; }
    }

    public static async Task<string> Announce(string node, string payload)
    {
        using var client = new HttpClient();
        var content = new StringContent("{\"payload\": \"" + payload + "\"}", Encoding.UTF8, "application/json");
        var response = await client.PutAsync(node + "/transactions", content);
        return await response.Content.ReadAsStringAsync();
    }
}