using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain.GameScreens;

public class PreGameScreen : IScreen
{
    private ConsoleService _consoleService = new ConsoleService();
    private GameManager _gameManager;
    private AppManager _appManager;
    
    public PreGameScreen(AppManager appManager)
    {
        _appManager = appManager;
        _gameManager = appManager.GameManager;
    }
    
    public bool HandleInput(string input)
    {
        switch (input)
        {
            case "1":
                _gameManager.ClientPlayer = new Player(Mark.X);
                break;
            case "2":
                _gameManager.ClientPlayer = new Player(Mark.O);
                break;
            case "3":
                Random random = new Random();
                _gameManager.ClientPlayer = new Player(random.Next(0, 2) == 0 ? Mark.X : Mark.O);
                break;
            default:
                _consoleService.SystemMessage("Invalid input. Please try again.");
                return false;
        }
        
        _gameManager.AddPlayer(_gameManager.ClientPlayer);

        if (_gameManager.OfflineMode)
        {
            // Then create Player 2 (Computer)
            Mark opponentMark = _gameManager.ClientPlayer.AssignedMark == Mark.X ? Mark.O : Mark.X;
            Player opponentPlayer = new Player(opponentMark);
            opponentPlayer.IsComputer = true;
            _gameManager.AddPlayer(opponentPlayer);
        }

        // Start the game
        _appManager.ChangeScreen(AppScreen.InGame);
        _gameManager.NewGame();
        
        return true;
    }

    public void OnEntry()
    {
        _consoleService.SystemMessage("Choose your mark.\n\t1 - X\n\t2 - O\n\t3 - Random");
    }

    public void OnExit()
    {
        //
    }
}