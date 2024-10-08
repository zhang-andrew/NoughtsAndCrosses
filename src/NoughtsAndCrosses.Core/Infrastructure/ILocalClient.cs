using NoughtsAndCrosses.Core.Constant;
using NoughtsAndCrosses.Core.Infrastructure.Domain;
using NoughtsAndCrosses.Core.Manager;

namespace NoughtsAndCrosses.Core.Infrastructure;

public interface ILocalClient
{
    public bool IsConnected { get; protected set; }
    
    public Task ConnectToWebSocket(string uri = Address.WebSocketUri);
    public Task DisconnectFromWebSocket();
    
    public Task SendGameEventAndWaitForResponse(GameManager gm, GameEvent gameEvent);

    public Task CreateLobbyAndWaitForOpponent(GameManager gm);
    
    public Task JoinLobbyAndStartGame(GameManager gm, OfflineLobby offlineLobby);
}