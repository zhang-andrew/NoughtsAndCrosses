using NoughtsAndCrosses.Core.Domain;

namespace NoughtsAndCrosses.Core.Infrastructure.Domain;

public abstract class LobbyBase
{
    Guid Id { get; }
    public LobbyState LobbyState { get; protected set; }
    public List<Player> Players { get; private set; } = new List<Player>();
    Game Game { get; }
    
    public LobbyBase()
    {
        LobbyState = LobbyState.WaitingForOpponent;
    }
    
    
    public void AddPlayer(Player player)
    {
        if (Players.Any(p => p.Id == player.Id))
        {
            throw new Exception($"Player already in lobby. Current player count: {Players.Count}");
        }

        Players.Add(player);
     
        if (Players.Count == 2 && Players.All(p => p.Id != player.Id))
        {
            LobbyState = LobbyState.ReadyToStart;
        }
    }
    
    public abstract Task StartGame();
}

public enum LobbyState
{
    WaitingForOpponent, 
    ReadyToStart, 
    GameInProgress
}