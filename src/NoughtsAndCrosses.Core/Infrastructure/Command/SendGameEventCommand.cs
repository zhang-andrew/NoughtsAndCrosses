using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;
using NoughtsAndCrosses.Core.Infrastructure.Domain;

namespace NoughtsAndCrosses.Core.Infrastructure;

public class SendGameEventCommand : WebSocketCommandBase
{
    private readonly GameEvent _gameEvent;

    public SendGameEventCommand(GameEvent gameEvent)
    {
        _gameEvent = gameEvent;
    }

    public override async Task Execute(WebSocket client)
    {
        string json = JsonConvert.SerializeObject(_gameEvent);
        byte[] sendBuffer = Encoding.UTF8.GetBytes(json);
        await client.SendAsync(new ArraySegment<byte>(sendBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
    }
}