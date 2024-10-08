using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Manager;
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
                _gameManager.ClientPlayer = new Player();
                _gameManager.ClientPlayer.AssignMark(Mark.X);
                break;
            case "2":
                _gameManager.ClientPlayer = new Player();
                _gameManager.ClientPlayer.AssignMark(Mark.O);
                break;
            case "3":
                Random random = new Random();
                _gameManager.ClientPlayer = new Player();
                _gameManager.ClientPlayer.AssignMark(random.Next(0, 2) == 0 ? Mark.X : Mark.O);
                break;
            default:
                return false;
        }

        
        
        if (_gameManager.IsOnline == false)
        {
            // Then create Player 2 (Computer)
            Mark opponentMark = _gameManager.ClientPlayer.AssignedMark == Mark.X ? Mark.O : Mark.X;
            Player opponentPlayer = new Player();
            opponentPlayer.AssignMark(opponentMark);
            opponentPlayer.IsComputer = true;
            
            // Start the game
            _appManager.ChangeScreen(AppScreen.InGame);
            _gameManager.NewLocalGame(_gameManager.ClientPlayer, opponentPlayer);
            
            return true;
        }

        // return false;
        throw new NotImplementedException("Online game is not implemented yet.");
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