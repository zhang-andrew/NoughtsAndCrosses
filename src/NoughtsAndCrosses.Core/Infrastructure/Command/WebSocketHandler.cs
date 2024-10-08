using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;

namespace NoughtsAndCrosses.Core.Infrastructure;

public class WebSocketHandler
{
    private readonly WebSocket _client;

    public WebSocketHandler(WebSocket client)
    {
        _client = client;
    }

    public async Task SendCommand(WebSocketCommandBase commandBase)
    {
        await commandBase.Execute(_client);
    }

    public async Task<WebSocketResponse> WaitForResponse()
    {
        var receiveBuffer = new byte[1024];
        var result = await _client.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
        
        // Convert the received byte array to a string
        string response = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);
        
        // convert the string to a json object
        WebSocketResponse webSocketResponse = JsonConvert.DeserializeObject<WebSocketResponse>(response);
        
        return webSocketResponse;
    }
}
