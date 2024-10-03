using NoughtsAndCrosses.Core.Enum;
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
        if (input == "back")
        {
            _appManager.ChangeScreen(AppScreen.Menu);
            return true;
        }

        if (input == "restart")
        {
            _appManager.ChangeScreen(AppScreen.PreGame);
            return true;
        }

        return false;
    }

    public void OnEntry()
    {
        if (_gameManager.Game.GetGameResult() == GameResult.Draw)
        {
            _consoleService.SystemMessage( $"Draw.\n\tType \"back\" to go back to the menu.\n\tType \"restart\" to restart the game.");
        } 
        else if (_gameManager.Game.GetGameResult() == GameResult.SomeoneWon)
        {
            if (_gameManager.ClientPlayer == _gameManager.Game.Winner)
            {
                _consoleService.SystemMessage( $"You WON with the \"{_gameManager.Game.Winner.AssignedMark}\" markers!\n\tType \"back\" to go back to the menu.\n\tType \"restart\" to restart the game.");
            }
            else
            {
                _consoleService.SystemMessage( $"You LOST, player with \"{_gameManager.Game.Winner.AssignedMark}\" markers won.\n\tType \"back\" to go back to the menu.\n\tType \"restart\" to restart the game.");
            }
        }
        else
        {
            // pass
        }
    }

    public void OnExit()
    {
        // Clean-up board
        _gameManager.ResetGame();
    }
}