using System.Net.WebSockets;

namespace NoughtsAndCrosses.Core.Infrastructure;

public class CreateLobbyCommand : WebSocketCommandBase
{
    public override Task Execute(WebSocket client)
    {
        throw new NotImplementedException();
    }
}