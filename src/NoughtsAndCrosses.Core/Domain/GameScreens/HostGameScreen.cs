using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Infrastructure;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain.GameScreens;

public class HostGameScreen : IScreen
{
    private ConsoleService _consoleService = new ConsoleService();
    private GameManager _gameManager;

    public HostGameScreen(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    
    public void HandleInputs(string input)
    {
        // wait for the other player to join from server
        
        throw new NotImplementedException();
    }
    
    public void OnEntry()
    {
        string serverUri = "ws://localhost:5148/ws"; 
        var playerClient = new PlayerClient(_consoleService);
        
        Task.Run(async () =>
        {
            try
            {
                await playerClient.ConnectToWebSocket(serverUri);
            }
            catch (Exception e)
            {
                _consoleService.UnhandledExceptionMessage(e);
                // throw; // we don't want to throw because we want to keep the app running
            }
        });
        
        // Generate random code, and send to server
        string randomCode = new Random().Next(1000, 9999).ToString();
        
        _consoleService.SystemMessage(GameScreen.HostGame, $"Share the following lobby code with your friend to join the game: {randomCode}");
    }

    public void OnExit()
    {
        _consoleService.SystemMessage(GameScreen.HostGame, $"Exiting \"{_gameManager.CurrentScreen}\" screen.");
    }
}