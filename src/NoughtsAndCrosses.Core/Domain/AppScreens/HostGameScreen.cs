using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Infrastructure;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain.GameScreens;

public class HostGameScreen : IScreen
{
    private ConsoleService _consoleService = new ConsoleService();
    private AppManager _appManager;

    public HostGameScreen(AppManager appManager)
    {
        _appManager = appManager;
    }
    
    public bool HandleInput(string input)
    {
        // wait for the other player to join from server
        _consoleService.SystemMessage($"Waiting for opponent to join.");
        
        return true;
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
        
        _consoleService.SystemMessage($"Share the following lobby code with your friend to join the game: {randomCode}");
    }

    public void OnExit()
    {
        //
    }
}