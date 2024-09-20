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
        throw new NotImplementedException();
    }

    public void OnEntry()
    {
        throw new NotImplementedException();
    }

    public void OnExit()
    {
        throw new NotImplementedException();
    }
}