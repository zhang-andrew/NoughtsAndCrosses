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
    
    public void HandleInputs(string input)
    {
        switch (input)
        {
            case MenuCommand.GoToOfflineGameScreen:
                _gameManager.ChangeScreen(GameScreen.InGameScreen);
                break;
            case MenuCommand.GoToHostScreen:
                _gameManager.ChangeScreen(GameScreen.HostGame);
                break;
            case MenuCommand.GoToJoinOnlineGame:
                _gameManager.ChangeScreen(GameScreen.JoinGame);
                break;
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