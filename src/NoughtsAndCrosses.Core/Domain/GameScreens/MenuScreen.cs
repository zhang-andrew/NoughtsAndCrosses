using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain.GameScreens;

public class MenuScreen : IScreen
{
    private ConsoleService _consoleService = new ConsoleService();
    private GameManager _gameManager;

    public MenuScreen(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    
    public bool HandleInput(string input)
    {
        switch (input)
        {
            case MenuCommand.GoToInGame:
                _gameManager.ChangeScreen(GameScreen.InGame);
                return true;
                break;
            case MenuCommand.GoToHostGame:
                _gameManager.ChangeScreen(GameScreen.HostGame);
                return true;
                break;
            case MenuCommand.GoToJoinGame:
                _gameManager.ChangeScreen(GameScreen.JoinGame);
                return true;
                break;
            default:
                return false;
        }
    }

    public void OnEntry()
    {
        _consoleService.SystemMessage(GameScreen.Menu, "Welcome to Noughts and Crosses.");
        _consoleService.SystemMessage(GameScreen.Menu, $"Type the number of the option you want to select.\n\t1 - Play Game\n\t2 - Host Game (Online)\n\t3 - Join Game (Online)");
    }

    public void OnExit()
    {
        _consoleService.SystemMessage(GameScreen.Menu, $"Exiting \"{_gameManager.CurrentScreen}\" screen.");
    }
}