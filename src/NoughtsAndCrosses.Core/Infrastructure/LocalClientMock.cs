using NoughtsAndCrosses.Core.Infrastructure.Domain;
using NoughtsAndCrosses.Core.Manager;

namespace NoughtsAndCrosses.Core.Infrastructure;

public class LocalClientMock : ILocalClient
{
    public bool IsConnected { get; set; }

    public Task ConnectToWebSocket(string uri)
    {
        IsConnected = true;
        
        return Task.CompletedTask;
    }

    public Task DisconnectFromWebSocket()
    {
        throw new NotImplementedException();
    }

    public Task SendGameEventAndWaitForResponse(GameManager gm, GameEvent gameEvent)
    {
        throw new NotImplementedException();
    }

    public Task CreateLobbyAndWaitForOpponent(GameManager gm)
    {
        throw new NotImplementedException();
    }

    public Task JoinLobbyAndStartGame(GameManager gm, OfflineLobby offlineLobby)
    {
        throw new NotImplementedException();
    }
}