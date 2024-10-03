using NoughtsAndCrosses.Core.Domain;

namespace NoughtsAndCrosses.WebSocketServer.Domain;

public class LobbyManager
{
    public List<Lobby> Lobbies { get; } = new List<Lobby>();
    
    public Lobby CreateLobby()
    {
        var lobby = new Lobby();
        Lobbies.Add(lobby);
        return lobby;
    }
    
    public Game GetGame(Guid lobbyId)
    {
        return Lobbies.First(l => l.Id == lobbyId).Game;
    }
    
    public Player AddWaitingPlayer(Guid lobbyId, Player player)
    {
        var lobby = Lobbies.First(l => l.Id == lobbyId);
        lobby.Players.Add(player);
        return player;
    }
    
    public void RemoveWaitingPlayer(Player player)
    {
        // Remove player from waiting list
        
    }
}