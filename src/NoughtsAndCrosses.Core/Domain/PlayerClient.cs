using System.Net.WebSockets;
using System.Text;

namespace NoughtsAndCrosses.Core.Domain;

public class PlayerClient
{
    private ClientWebSocket _client = new();
    
    public PlayerClient()
    {
    }

    public async Task ConnectToWebSocket(string uri) // e.g. "ws://localhost:5000/ws" or "wss://localhost:5001/ws"
    {
        _client = new ClientWebSocket();
        await _client.ConnectAsync(new Uri(uri), CancellationToken.None);
        Console.WriteLine("Connected!");
        
        await SendToWebSocket("helllllllllllo");
        

        // var sendBuffer = Encoding.UTF8.GetBytes("Hello Server!");
        // await _client.SendAsync(new ArraySegment<byte>(sendBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
        //
        // var receiveBuffer = new byte[1024];
        // var result = await _client.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
        // Console.WriteLine($"Received: {Encoding.UTF8.GetString(receiveBuffer, 0, result.Count)}");
        //
        // var sendBuffer2 = Encoding.UTF8.GetBytes("Hello Server! Blah");
        // await _client.SendAsync(new ArraySegment<byte>(sendBuffer2), WebSocketMessageType.Text, true, CancellationToken.None);
        //
        // var result2 = await _client.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
        // Console.WriteLine($"Received: {Encoding.UTF8.GetString(receiveBuffer, 0, result2.Count)}");
        //
        // await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
    }

    public async Task SendToWebSocket(string input)
    {
        // _client = new ClientWebSocket();
        // await _client.ConnectAsync(new Uri(uri), CancellationToken.None);
        // Console.WriteLine("Connected!");
        
        var sendBuffer = Encoding.UTF8.GetBytes(input);
        await _client.SendAsync(new ArraySegment<byte>(sendBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
        
        var receiveBuffer = new byte[1024];
        var result = await _client.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
        Console.WriteLine($"Received from Server: {Encoding.UTF8.GetString(receiveBuffer, 0, result.Count)}");
    }
}