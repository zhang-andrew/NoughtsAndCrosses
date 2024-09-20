using System.Net.WebSockets;
using System.Text;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain;

public class PlayerClient
{
    private ClientWebSocket _client = new();
    private ConsoleService _consoleService;

    public PlayerClient(ConsoleService consoleService)
    {
        _consoleService = consoleService;
    }

    public async Task ConnectToWebSocket(string uri) // e.g. "ws://localhost:5000/ws" or "wss://localhost:5001/ws"
    {
        try
        {
            _client = new ClientWebSocket();
            await _client.ConnectAsync(new Uri(uri), CancellationToken.None);
            Console.WriteLine("Connected!");

            await SendToWebSocket("helllllllllllo");
        }
        catch (WebSocketException e)
        {
            _consoleService.HandledExceptionMessage(e, "Ensure the server is running and the URI is correct.");
        } 
        catch (Exception e)
        {
            throw;
        }
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