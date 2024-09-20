using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain.GameScreens;

public class JoinGameScreen : IScreen
{
    private ConsoleService _consoleService = new ConsoleService();
    private GameManager _gameManager;

    public JoinGameScreen(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    
    public void HandleInputs(string input)
    {
        _consoleService.SystemMessage(GameScreen.JoinGame, $"You typed {input}");
        // throw new NotImplementedException();
    }

    public void OnEntry()
    {
        _consoleService.SystemMessage(GameScreen.JoinGame, "Enter the lobby code to join the game");
        // throw new NotImplementedException();
    }

    public void OnExit()
    {
        _consoleService.SystemMessage(GameScreen.JoinGame, $"Exiting \"{_gameManager.CurrentScreen}\" screen.");
        // throw new NotImplementedException();
    }
}