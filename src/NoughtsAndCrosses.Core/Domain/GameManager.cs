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
    
    public Board Board;
    private List<Player> _players = new() { };
    public Player? ClientPlayer = null; // The player that is playing on the local machine
    public Player? TurnPlayer = null;
    public bool OfflineMode = true;
    private ConsoleService _consoleService = new ConsoleService();

    private GameManager() // private ctor is optional but enforces the developer to not create new instance. Use the public Instance property instead.
    {
    }

    public void AddPlayer(Player player)
    {
        _players.Add(player);
    }

    public void StartGame(Mark? clientAssignedMark = null)
    {
        // _players must have at least 2 players
        if (_players.Count < 2)
        {
            throw new InvalidOperationException("There must be at least 2 players to start a game.");
        }
        
        Board = new Board();
        
        // Assign the X player to go first
        TurnPlayer = _players.First(p => p.AssignedMark == Mark.X); // X always goes first

        // Notify all players
        foreach (var player in _players)
        {
            player.NotifyPlayerMark();
            if (player == TurnPlayer)
            {
                player.NotifyTurn();
            }
            else
            {
                player.NotifyWait();
            }
        }
        
        Board.ShowBoard();
    }

    public void HostGame()
    {
    }

    public void JoinGame()
    {
        ClientPlayer.NotifyPlayerMark();
    }

    public void NextTurn()
    {
        TurnPlayer = TurnPlayer == _players[0] ? _players[1] : _players[0];
    }

    public void HandleTurn()
    {
        // Broadcast messages
        foreach (var player in _players)
        {
            if (player == ClientPlayer)
            {
                player.IsTheirTurn = true;
                player.NotifyTurn();
            }
            else
            {
                player.IsTheirTurn = false;
                player.NotifyWait();
            }
        }
    }

    public void Reset()
    {
        
    }


}