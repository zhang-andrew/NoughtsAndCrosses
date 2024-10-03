using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Manager;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain.GameScreens;

public class JoinGameScreen : IScreen
{
    private ConsoleService _consoleService = new ConsoleService();
    private AppManager _appManager;

    public JoinGameScreen(AppManager appManager)
    {
        _appManager = appManager;
    }
    
    public bool HandleInput(string input)
    {
        _consoleService.SystemMessage( $"You typed {input}");
        
        // Check if input is a valid lobby code
        // Else false
        return false;
    }

    public void OnEntry()
    {
        _consoleService.SystemMessage( "\n\tEnter the \"Lobby Code\" to join game. \n\tType \"back\" to return to the menu.");
    }

    public void OnExit()
    {
        //
    }
}