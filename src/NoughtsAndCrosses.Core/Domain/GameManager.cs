using NoughtsAndCrosses.Core.Domain.GameScreens;
using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain;

public class GameManager
{
    // Singleton implementation ()
    private static readonly GameManager _instance = new GameManager();
    public static GameManager Instance
    {
        get { return _instance; }
    }
    
    public Game? Game { get; private set; }
    public List<Player> Players { get; private set; } = new() { };
    public Player? ClientPlayer = null; // The player that is playing on the local machine
    public Player? TurnPlayer = null;
    public bool OfflineMode = true;
    private ConsoleService _consoleService = new ConsoleService();

    private GameManager() // private ctor is optional but enforces the developer to not create new instance. Use the public Instance property instead.
    {
    }

    public Player AddPlayer(Player player)
    {
        Players.Add(player);
        return player;
    }

    public void NewGame()
    {
        // _players must have at least 2 players
        if (Players.Count < 2)
        {
            throw new InvalidOperationException($"There must be at least 2 players to start a game.");
        }
        
        // Setup
        Game = new Game();
        Players.ForEach(p => p.NotifyPlayerMark());
        TurnPlayer = Players.First(p => p.AssignedMark == Mark.X); // Assign the X player to go first
        
        HandleTurn();
    }
    
    public void NextTurn()
    {
        TurnPlayer = TurnPlayer == Players[0] ? Players[1] : Players[0];
        HandleTurn();
    }

    /// <summary>Notifies all players to move or wait, if player is computer and it's their turn, it will make them move immediately.</summary>
    private void HandleTurn()
    {
        if (Game.CheckGameResult() != GameResult.InProgress)
        {
            // End game
            Game.ShowBoard();
            AppManager.Instance.ChangeScreen(AppScreen.PostGame);
            return;
        }
        
        // Notify all players
        foreach (var player in Players)
        {
            if (player == ClientPlayer)
            {
                Game.ShowBoard();
            }
            
            if (player == TurnPlayer)
            {
                player.NotifyTurn();
            }
            else
            {
                player.NotifyWait();
            }
        }
    }

    public void HostGame()
    {
    }

    public void JoinGame()
    {
        ClientPlayer.NotifyPlayerMark();
    }


    public void ResetGame()
    {
        Game = null;
        
        this.Players.Clear();
        
        this.ClientPlayer = null;
        
        this.TurnPlayer = null;
    }


}