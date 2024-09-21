using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain;

public class Player
{
    public Mark AssignedMark;
    public bool IsTheirTurn;
    private bool IsComputer = false;
    
    private ConsoleService _consoleService;
    public Guid Id { get; } = Guid.NewGuid();

    public Player(Mark assignedMark, bool isComputer = false)
    {
        IsComputer = isComputer;
        AssignedMark = assignedMark;
        _consoleService = new ConsoleService();
    }
    
    public void NotifyPlayerMark()
    {
        _consoleService.SystemMessage($"You are playing as {AssignedMark}");
    }
    
    public void NotifyTurn()
    {
        if (IsComputer)
        {
            // Make a move
        } else
        {
            _consoleService.SystemMessage("It's your turn!");
        }
    }

    public void NotifyWait()
    {
        _consoleService.SystemMessage("Waiting for other player to make a move...");
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