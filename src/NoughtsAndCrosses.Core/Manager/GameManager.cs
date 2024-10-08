using NoughtsAndCrosses.Core.Domain;
using NoughtsAndCrosses.Core.Domain.GameScreens;
using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Infrastructure;
using NoughtsAndCrosses.Core.Infrastructure.Domain;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Manager;

public class GameManager
{
    // Singleton implementation ()
    private static readonly GameManager _instance = new GameManager();
    public static GameManager Instance
    {
        get { return _instance; }
    }
    
    private ConsoleService _consoleService = new ConsoleService();
    
    public bool IsOnline = false;
    private Player? _clientPlayer { get; set; } // The player that is playing on the local machine
    private Game? _game { get; set; }
    public ILocalClient LocalClient { get; private set; }
    

    private GameManager() // private ctor is optional but enforces the developer to not create new instance. Use the public Instance property instead.
    {
        LocalClient = new LocalClient();
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

    

    public void NewOnlineGame()
    {
        
    }

    public void NewLocalGame(Player player1, Player player2)
    {
        // _players must have at least 2 players
        if (player1 == null || player2 == null)
        {
            throw new InvalidOperationException($"There must be at least 2 players to start a game.");
        }
        if (_clientPlayer == null)
        {
            throw new NullReferenceException($"ClientPlayer must be assigned before starting a game.");
        }
        
        // Setup
        
        Game = new Game(player1, player2);
        // Game.AddPlayer(player1);
        // Game.AddPlayer(player2);
        Game.Players.ForEach(p => p.NotifyPlayerMark());
        Game.TurnPlayer = Game.Players.First(p => p.AssignedMark == Mark.X); // Assign the X player to go first
        
        CheckWinConditionAndNotifyPlayers();
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
        foreach (var player in Game.Players)
        {
            if (player == ClientPlayer)
            {
                Game.ShowBoard();
            }
            
            if (player == Game.TurnPlayer)
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
        
        // this.Players.Clear();
        
        this.ClientPlayer = null;
        
        // this.TurnPlayer = null;
    }


    public async Task<LobbyBase> CreateLobby(Player hostPlayer, bool isOnline)
    {
        if (isOnline)
        {
            // Create localClient and connect to server
            await LocalClient.ConnectToWebSocket();
            
            OnlineLobby onlineLobby = new OnlineLobby();
            onlineLobby.AddPlayer(hostPlayer);
            return onlineLobby;
        }
        else
        {
            OfflineLobby offlineLobby = new OfflineLobby();
            offlineLobby.AddPlayer(hostPlayer);
            return offlineLobby;
        }
    }

    public void UseMockLocalClient()
    {
        LocalClient = new LocalClientMock();
    }
}