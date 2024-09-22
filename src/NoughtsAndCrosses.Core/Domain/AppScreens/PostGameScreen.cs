using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain.GameScreens;

public class PostGameScreen : IScreen
{
    private AppManager _appManager;
    private GameManager _gameManager;
    private ConsoleService _consoleService = new ConsoleService();

    public PostGameScreen(AppManager appManager)
    {
        _appManager = appManager;
        _gameManager = appManager.GameManager;
    }

    public bool HandleInput(string input)
    {
        return true;
    }

    public void OnEntry()
    {
        // Show message based on the game result
        _consoleService.SystemMessage( $"Game over. \"{_gameManager.Board.GetWinner()}\" wins.\n\tType \"back\" to go back to the menu.\n\tType \"restart\" to restart the game.");
    }

    public void OnExit()
    {
        // Clean-up board
        
        
    }
}