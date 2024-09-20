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
    
    public bool HandleInput(string input)
    {
        _consoleService.SystemMessage(GameScreen.JoinGame, $"You typed {input}");
        
        // Check if input is a valid lobby code
        // Else false
        return false;
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