using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Infrastructure;
using NoughtsAndCrosses.Core.Manager;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain.GameScreens;

public class HostGameScreen : IScreen
{
    private ConsoleService _consoleService = new ConsoleService();
    private AppManager _appManager;

    private bool IsConnected = false;
    private LocalClient _localClient;

    public HostGameScreen(AppManager appManager)
    {
        _appManager = appManager;
        
        _localClient = new LocalClient();
    }
    
    public bool HandleInput(string input)
    {
        if (!IsConnected)
        {
            _consoleService.SystemMessage("Connecting to server. Please wait...");
            return true;
        }
        else
        {
            _localClient.SendToWebSocket(input);
        }

        // wait for the other player to join from server
        _consoleService.SystemMessage($"Waiting for opponent to join.");
        
        return false;
    }
    
    public void OnEntry()
    {
        Task.Run(async () =>
        {
            try
            {
                bool connected = await _localClient.ConnectToWebSocket();
                
                if (connected)
                {
                    IsConnected = true;
                    
                    // Generate random code, and send to server
                    string randomCode = new Random().Next(1000, 9999).ToString();
                    _consoleService.SystemMessage($"Share the following lobby code with your friend to join the game: {randomCode}");
                } else {
                    _consoleService.SystemMessage("Failed to connect to server. Type \"back\" to return to the menu.");
                    // _appManager.ChangeScreen(AppScreen.Menu);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unhandled exception occurred. Exiting...");
                throw; // Rethrow the exception
            }
        });
    }

    public void OnExit()
    {
        //
    }
}