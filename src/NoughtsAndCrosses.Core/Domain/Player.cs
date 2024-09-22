using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain;

public class Player
{
    public Mark AssignedMark { get; }
    public bool IsComputer = false;
    public Guid Id { get; } = Guid.NewGuid();
    
    private ConsoleService _consoleService;
    private GameManager _gameManager = GameManager.Instance;

    public Player(Mark assignedMark, bool isComputer = false)
    {
        IsComputer = isComputer;
        AssignedMark = assignedMark;
        _consoleService = new ConsoleService();
    }
    
    public void NotifyPlayerMark()
    {
        if (_gameManager.ClientPlayer == this)
        {
            _consoleService.SystemMessage($"You are playing as {AssignedMark}");
        }
    }
    
    public void NotifyTurn()
    {
        if (IsComputer)
        {
            // Wait and delay 2 seconds
            // Task.Delay(2000).Wait();
            // Console.WriteLine("hi");
            // Console.WriteLine(_gameManager.Board.Spaces);
            _gameManager.Board.PlaceMarkRandomly(AssignedMark);
            // _gameManager.Board.ShowBoard();
        } else
        {
            _consoleService.SystemMessage("It's your turn! (Type the coordinate to place a mark.)");
        }
    }

    public void NotifyWait()
    {
        if (_gameManager.ClientPlayer == this)
        {
            _consoleService.SystemMessage("Waiting for opponent to move.");
        }
    }

    public void NotifyWin()
    {
        _consoleService.SystemMessage("You win!");
    }
    
    public void NotifyLoss()
    {
        _consoleService.SystemMessage("You lost!");
    }
}