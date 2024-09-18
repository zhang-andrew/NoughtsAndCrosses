using System.Net.WebSockets;
using System.Text;

namespace NoughtsAndCrosses.WebSocketServer.Domain;

public class Server
{
    private WebApplication _app;

    public Server()
    {
        Console.WriteLine("running");
    }

    public void Start(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        _app = builder.Build();
        
        _app.UseWebSockets(); 

        _app.MapGet("/", () => "Hello World!");
        _app.Map("/ws", async context =>
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                Console.WriteLine("New client connected");

                await HandleWebSocketConnection(webSocket);
            }
            else
            {
                Console.WriteLine("Not a websocket request");
                context.Response.StatusCode = 400;
            }
        });



        _app.Run();
    }
    
    
    private async Task HandleWebSocketConnection(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        while (!result.CloseStatus.HasValue)
        {
            Console.WriteLine($"Received from Client: {Encoding.UTF8.GetString(buffer, 0, result.Count)}");
            Console.WriteLine($"Broadcasting to all Clients: {Encoding.UTF8.GetString(buffer, 0, result.Count)}");
            await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }
        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }
}