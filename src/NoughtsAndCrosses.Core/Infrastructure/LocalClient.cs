using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;
using NoughtsAndCrosses.Core.Constant;
using NoughtsAndCrosses.Core.Domain;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Infrastructure;

public class LocalClient
{
    private ClientWebSocket _client = new();
    private ConsoleService _consoleService;

    public LocalClient()
    {
        _consoleService = new ConsoleService();
    }

    public async Task<bool> ConnectToWebSocket(string uri = Address.WebSocketUri) // e.g. "ws://localhost:5000/ws" or "wss://localhost:5001/ws"
    {
        try
        {
            _client = new ClientWebSocket();
            await _client.ConnectAsync(new Uri(uri), CancellationToken.None);
            Console.WriteLine("Connected!");

            await SendToWebSocket("helllllllllllo");

            return true;
        }
        catch (WebSocketException e)
        {
            _consoleService.HandledExceptionMessage(e, "Ensure the server is running and the URI is correct.");
            return false;
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
    
    public async Task SendGameMoveToWebSocket(string input)
    {
        
        var jsonObject = new { player = "playerX", message = "" };
        string jsonString = JsonConvert.SerializeObject(jsonObject);
        
        
        var sendBuffer = Encoding.UTF8.GetBytes(input);
        await _client.SendAsync(new ArraySegment<byte>(sendBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
    }
    
    public async Task<Game> SendGetGameRequestToWebSocket(Guid lobbyId)
    {
        // Get lobby from server via websocket request

        throw new NotImplementedException();
    }
    
    public async Task<Guid> SendCreateLobbyRequestToWebSocket()
    {
        // Send lobby creation request to server via websocket

        throw new NotImplementedException();
    }
}