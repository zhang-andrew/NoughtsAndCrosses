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
    private AppManager _appManager = AppManager.Instance;

    public Player(Mark assignedMark, bool isComputer = false)
    {
        IsComputer = isComputer;
        AssignedMark = assignedMark;
        _consoleService = new ConsoleService();
    }
    
    
    public void PlaceMark(Coordinate coordinate)
    {
        if (!IsAllowedToPlaceMark().Key)
        {
            string message = IsAllowedToPlaceMark().Value;
            throw new Exception(message);
        }

        Space space = _gameManager.Game.Spaces.First(s => s.Coordinate.Value == coordinate.Value);
        space.Mark = AssignedMark;
        _consoleService.SystemMessage($"Player \"{AssignedMark}\" marked \"{space.Coordinate.Value}\".");
        
        // if (WasWinningMove()) return;
        
        _gameManager.Game.NextTurn();
        _gameManager.CheckWinConditionAndNotifyPlayers();
    }
    
    private void PlaceMarkRandomly(Mark mark)
    {
        if (!IsAllowedToPlaceMark().Key)
        {
            string message = IsAllowedToPlaceMark().Value;
            throw new Exception(message);
        }
        
        var spacesWithoutMarks = _gameManager.Game.Spaces.Where(s => s.Mark == Mark.Empty).ToArray();
        Random random = new Random();
        int randomInt = random.Next(0, spacesWithoutMarks.Length);
        var randomUnmarkedSpace = _gameManager.Game.Spaces.First(s => s.Coordinate.Value == spacesWithoutMarks[randomInt].Coordinate.Value);
        randomUnmarkedSpace.Mark = mark;
        _consoleService.SystemMessage($"Opponent marked \"{randomUnmarkedSpace.Coordinate.Value}\".");
        
        // if (WasWinningMove()) return;
        
        _gameManager.Game.NextTurn();
        _gameManager.CheckWinConditionAndNotifyPlayers();
    }
    
    private void PlaceBestMark(Mark mark) // for Computer Player
    {
        // Check for winning move
        
        // Stop player from winning
        
        // Best setup for winning move
        
        // Go for middle square if available
        
        // Go for corner square if available
        
        // Space bestSpace = GetBestSpace();
        // bestSpace.Mark = mark;

        throw new NotImplementedException();
    }
    
    
    private bool WasWinningMove()
    {
        if (_gameManager.Game.GetGameResult() != GameResult.InProgress)
        {
            // _appManager.ChangeScreen(AppScreen.PostGame);
            return true;
        }

        return false;
    }

    private KeyValuePair<bool, string> IsAllowedToPlaceMark()
    {
        if (_appManager.CurrentScreen != AppScreen.InGame)
        {
            return new (false, "Invalid screen. You can only place a mark in the game screen.");
        }
        
        if (_gameManager.Game.GetGameResult() is GameResult.SomeoneWon or GameResult.Draw)
        {
            return new (false, "Game has already ended. You can't place a mark.");
        }
        
        if (_gameManager.Game.TurnPlayer != this)
        {
            return new (false, "It's not your turn.");
        }
        
        
        return new(true, "");
    }
    
    public void NotifyPlayerMark()
    {
        if (_gameManager.ClientPlayer == this)
        {
            _consoleService.SystemMessage($"You have the \"{AssignedMark}\" markers.");
        }
    }
    
    public void NotifyTurn()
    {
        if (IsComputer)
        {
            // Wait and delay 2 seconds
            Task.Delay(1000).Wait();
            PlaceMarkRandomly(AssignedMark);
            // _gameManager.Board.ShowBoard();
        } else
        {
            _consoleService.SystemMessage($"Your turn! Mark any spot with \"{AssignedMark}\" by typing the coordinate (e.g. a1).");
                
        }
    }

    public void NotifyWait()
    {
        if (_gameManager.ClientPlayer == this)
        {
            _consoleService.SystemMessage("Opponent's turn.");
            // _gameManager.Game.ShowBoard();
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