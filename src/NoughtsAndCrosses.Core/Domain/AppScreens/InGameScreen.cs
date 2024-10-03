using NoughtsAndCrosses.Core.Constant;
using NoughtsAndCrosses.Core.Domain.Exceptions;
using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Manager;
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
        try
        {
            Coordinate parsedCoordinate = Coordinate.Parse(input.ToUpper());
            _gameManager.ClientPlayer.PlaceMark(parsedCoordinate);
            return true;
        }
        catch (InvalidCoordinateException e)
        {
            _consoleService.HandledExceptionMessage(e, "Please enter a valid coordinate.");
            return false;
        }
        catch (SpaceOccupiedException e)
        {
            _consoleService.HandledExceptionMessage(e, "Enter a coordinate that is empty.");
            return false;
        }
    }

    public void OnEntry()
    {
        // IF - Confirm there's two players first
        
        // _gameManager.NewGame();
    }
    
    public void OnExit()
    {
        //
    }
}