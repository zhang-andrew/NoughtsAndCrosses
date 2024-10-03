using NoughtsAndCrosses.Core.Domain;

namespace NoughtsAndCrosses.Core.Service;

public class LobbyService
{
    public List<Lobby> Lobbies { get; } = new List<Lobby>();
    
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

public class Lobby
{
    public Guid Id { get; } = Guid.NewGuid();
    public List<Player> Players { get; } = new List<Player>();
    public Game Game { get; set; }
}