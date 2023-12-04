using System.Net.WebSockets;
using Newtonsoft.Json;

namespace aLice_utils.Client.Services;

public class WebSocketService
{
    private ClientWebSocket? webSocket;
    private Task? receiveLoopTask;

    public event Action<string>? Received;

    public async Task ConnectAsync(string serverUri)
    {
        webSocket = new ClientWebSocket();
        var uri = new Uri(serverUri);
        await webSocket.ConnectAsync(uri, CancellationToken.None);

        // メッセージ受信を監視するための非同期ループを開始
        receiveLoopTask = ReceiveLoop();
    }

    private async Task ReceiveLoop()
    {
        while (webSocket is {State: WebSocketState.Open})
        {
            var buffer = new ArraySegment<byte>(new byte[4096]);
            var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text)
            {
                if (buffer.Array != null)
                {
                    var message = System.Text.Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                    Received?.Invoke(message);
                }
            }
        }
    }

    public async Task SendAsync(object data)
    {
        if (webSocket is {State: WebSocketState.Open})
        {
            var json = JsonConvert.SerializeObject(data);
            var buffer = new ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(json));
            await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

    public async Task DisconnectAsync()
    {
        if (webSocket is {State: WebSocketState.Open})
        {
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by client", CancellationToken.None);
            if (receiveLoopTask != null) await receiveLoopTask;
        }
    }
}