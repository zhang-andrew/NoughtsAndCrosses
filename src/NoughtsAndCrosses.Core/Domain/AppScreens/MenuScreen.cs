using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain.GameScreens;

public class MenuScreen : IScreen
{
    private ConsoleService _consoleService = new ConsoleService();
    private AppManager _appManager;

    public MenuScreen(AppManager appManager)
    {
        _appManager = appManager;
    }
    
    public bool HandleInput(string input)
    {
        switch (input)
        {
            case MenuCommand.GoToPreGame:
                _appManager.ChangeScreen(AppScreen.PreGame);
                return true;
                break;
            case MenuCommand.GoToHostGame:
                _appManager.ChangeScreen(AppScreen.HostGame);
                return true;
                break;
            case MenuCommand.GoToJoinGame:
                _appManager.ChangeScreen(AppScreen.JoinGame);
                return true;
                break;
            default:
                return false;
        }
    }

    public void OnEntry()
    {
        _consoleService.SystemMessage("Welcome to Noughts and Crosses.");
        _consoleService.SystemMessage($"Type the number of the option you want to select.\n\t1 - Play Game\n\t2 - Host Game (Online)\n\t3 - Join Game (Online)");
    }

    public void OnExit()
    {
        //
    }
}