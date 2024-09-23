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
    
    public List<Player> Players { get; private set; } = new() { };
    public Player? TurnPlayer { get; private set; }
    public bool OfflineMode = true;
    private ConsoleService _consoleService = new ConsoleService();
    private Player? _clientPlayer { get; set; } // The player that is playing on the local machine
    private Game? _game { get; set; }

    private GameManager() // private ctor is optional but enforces the developer to not create new instance. Use the public Instance property instead.
    {
    }
    
    public Player? ClientPlayer
    {
        get
        {
            if (_clientPlayer == null)
            {
                throw new NullReferenceException("GameManager.ClientPlayer is null, assign it manually or normally assigned in PreGameScreen.");
            }
            return _clientPlayer;
        }
        set => _clientPlayer = value;
    } 
    
    public Game? Game
    {
        get
        {
            if (_game == null)
            {
                throw new NullReferenceException("GameManager.Game is null, use NewGame() method to start one.");
            }
            return _game;
        }
        private set => _game = value;
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
        if (_clientPlayer == null)
        {
            throw new NullReferenceException($"ClientPlayer must be assigned before starting a game.");
        }
        
        // Setup
        Game = new Game();
        Players.ForEach(p => p.NotifyPlayerMark());
        TurnPlayer = Players.First(p => p.AssignedMark == Mark.X); // Assign the X player to go first
        
        CheckWinConditionAndNotifyPlayers();
    }
    
    public void NextTurn()
    {
        TurnPlayer = TurnPlayer == Players[0] ? Players[1] : Players[0];
    }

    /// <summary>Notifies all players to move or wait, if player is computer and it's their turn, it will make them move immediately.</summary>
    public void CheckWinConditionAndNotifyPlayers()
    {
        if (Game.GetGameResult() != GameResult.InProgress)
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