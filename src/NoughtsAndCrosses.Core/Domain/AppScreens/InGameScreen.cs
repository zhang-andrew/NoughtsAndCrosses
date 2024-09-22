using NoughtsAndCrosses.Core.Constant;
using NoughtsAndCrosses.Core.Domain.Exceptions;
using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain.GameScreens;

public class InGameScreen : IScreen
{
    private ConsoleService _consoleService = new ConsoleService();
    private GameManager _gameManager = GameManager.Instance;
    private AppManager _appManager;

    private string[] _allowedCommands;

    public InGameScreen(AppManager appManager)
    {
        _appManager = appManager;
    }
    
    public bool HandleInput(string input)
    {
        Coordinate parsedCoordinate = Coordinate.Parse(input.ToUpper());
        _gameManager.Board.PlaceMark(parsedCoordinate, _gameManager.ClientPlayer.AssignedMark);
        
        if (_gameManager.Board.HasWinner() || _gameManager.Board.HasDraw())
        {
            _appManager.ChangeScreen(AppScreen.PostGame);
            return true;
        }
        
        _gameManager.Board.ShowBoard();
        return true;
    }

    public void OnEntry()
    {
        // IF - Confirm there's two players first
        
        _gameManager.StartGame();
    }
    
    public void OnExit()
    {
        //
    }
}