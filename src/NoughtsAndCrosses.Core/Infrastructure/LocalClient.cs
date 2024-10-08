using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;
using NoughtsAndCrosses.Core.Constant;
using NoughtsAndCrosses.Core.Domain;
using NoughtsAndCrosses.Core.Infrastructure.Domain;
using NoughtsAndCrosses.Core.Manager;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Infrastructure;

public class LocalClient : ILocalClient
{
    public bool IsConnected { get; set; }
    private ClientWebSocket _client = new();
    private ConsoleService _consoleService;

    public LocalClient()
    {
        _consoleService = new ConsoleService();
    }


    public async Task ConnectToWebSocket(string uri = Address.WebSocketUri) // e.g. "ws://localhost:5000/ws" or "wss://localhost:5001/ws"
    {
        try
        {
            _client = new ClientWebSocket();
            await _client.ConnectAsync(new Uri(uri), CancellationToken.None);
            
            Console.WriteLine("Connected!");
            // await SendToWebSocket("helllllllllllo");

            IsConnected = true;
        }
        catch (WebSocketException e)
        {
            // _consoleService.HandledExceptionMessage(e, "Ensure the server is running and the URI is correct.");
            throw new WebSocketException("Ensure the server is running and the URI is correct.");
        } 
    }
    
    public async Task DisconnectFromWebSocket()
    {
        try
        {
            await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing connection", CancellationToken.None);
            
            Console.WriteLine("Disconnected!");
            
            IsConnected = false;
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
    
    public async Task SendGameEventAndWaitForResponse(GameManager gm, GameEvent gameEvent)
    {
        var gameEventCommand = new SendGameEventCommand(gameEvent);
        var webSocketHandler = new WebSocketHandler(_client);
    
        // Send the move
        await webSocketHandler.SendCommand(gameEventCommand);
    
        // Wait for the next move or other game event
        WebSocketResponse response = await webSocketHandler.WaitForResponse();
        
        if (response.Success)
        {
            // Deserialize the data to move
            GameEvent receivedGameEvent = JsonConvert.DeserializeObject<GameEvent>(response.UnserializedData);
            
            // Update the game state
            switch (receivedGameEvent.GameEventType)
            {
                case GameEventType.MoveMade:
                    Player opponent = gm.Game.Players.First(p => p.Id == receivedGameEvent.PlayerWhoMadeMove.Id);
                    opponent.PlaceMark(receivedGameEvent.Coordinate);
                    break;
                
            }
        }
    }
    
    public async Task CreateLobbyAndWaitForOpponent(GameManager gm)
    {
        var createLobbyCommand = new CreateLobbyCommand();
        var webSocketHandler = new WebSocketHandler(_client);
    
        // Create the lobby
        await webSocketHandler.SendCommand(createLobbyCommand);
    
        // Wait for the opponent to join
        WebSocketResponse response = await webSocketHandler.WaitForResponse();
        
        if (response.Success)
        {
            // Deserialize the data to lobby
            // var json = 
            
            // gm.SetOpponent(response.UnserializedData);
        }
    }

    public Task JoinLobbyAndStartGame(GameManager gm, OfflineLobby offlineLobby)
    {
        throw new NotImplementedException();
    }


    // public async Task SendRequestToWebSocket(WebSocketRequest request)
    // {
    //     // Serialize move object to json
    //     string json = JsonConvert.SerializeObject(request);
    //     
    //     // Convert json to byte array and send to server
    //     byte[] sendBuffer = Encoding.UTF8.GetBytes(json);
    //     
    //     // Send move to server via websocket
    //     await _client.SendAsync(new ArraySegment<byte>(sendBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
    // }
    //
    // public async Task CreateLobbyAndWaitForOpponent(GameManager gm)
    // {
    //     // Serialize move object to json
    //     string json = JsonConvert.SerializeObject(new WebSocketRequest()
    //     {
    //         RequestType = RequestType.CreateLobby,
    //         Data = move
    //     });
    //     
    //     // Convert json to byte array and send to server
    //     byte[] sendBuffer = Encoding.UTF8.GetBytes(json);
    //     
    //     // Send move to server via websocket
    //     await _client.SendAsync(new ArraySegment<byte>(sendBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
    //     
    //     
    //     // Wait for opponent to join
    //     var receiveBuffer = new byte[1024];
    //     var result = await _client.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
    //     Console.WriteLine($"Received from Server: {Encoding.UTF8.GetString(receiveBuffer, 0, result.Count)}");
    //     
    //     
    //     // Deserialize json to object
    //     
    //     // RequestType is 
    // }
    //
    // public async Task SendMoveAndWaitForNextMove(GameManager gm, Move move)
    // {
    //     // Serialize move object to json
    //     string json = JsonConvert.SerializeObject(new WebSocketRequest()
    //     {
    //         RequestType = RequestType.MakeMove,
    //         Data = move
    //     });
    //     
    //     // Convert json to byte array and send to server
    //     byte[] sendBuffer = Encoding.UTF8.GetBytes(json);
    //     
    //     // Send move to server via websocket
    //     await _client.SendAsync(new ArraySegment<byte>(sendBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
    //     
    //     
    //     // Wait for next move from server
    //     var receiveBuffer = new byte[1024];
    //     var result = await _client.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
    //     Console.WriteLine($"Received from Server: {Encoding.UTF8.GetString(receiveBuffer, 0, result.Count)}");
    // }

}