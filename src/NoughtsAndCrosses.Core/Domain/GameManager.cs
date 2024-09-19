// using NoughtsAndCrosses.Core.Service;

using System.Runtime.InteropServices.JavaScript;
using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain;

public class GameManager
{
    public AppState AppState = AppState.MainMenu;
    public Player? LocalPlayer = null;
    
    private ConsoleService _consoleService;
    
    public GameState? GameState = null;


    public GameManager()
    {
        _consoleService = new ConsoleService();
        
        GameState = new GameState();
    }

    public async void Run()
    {
        _consoleService.SystemMessage("Welcome to Noughts and Crosses");
        _consoleService.SystemMessage("1 - New offline game, 2 - New online game, 3 - Join online game");

        await ListenForInputs();
    }

    private async Task ListenForInputs()
    {
        bool running = true;
        while (running)
        {
            try
            {
                string? input = Console.ReadLine();
            
                if (input == "exit")
                {
                    running = false;
                    // break;
                }
            
                switch (AppState)
                {
                    case AppState.MainMenu:
                        HandleMenuInput(input);
                        break;
                    case AppState.InOfflineGame:
                    case AppState.InOnlineGame:
                        HandleGameInput(input);
                        break;
                    default:
                        _consoleService.SystemMessage("Invalid application state.");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // throw;
            }
        }
    }


    
    public void HandleMenuInput(string input)
    {
        switch (input)
        {
            case "1":
                NewOfflineGame();
                break;
            case "2":
                NewOnlineGame("ws://localhost:5148/ws");
                break;
            case "3":
                // JoinOnlineGame("ws://localhost:5148/ws", "gameId");
                break;
        }
    }
    
    
    public void HandleGameInput(string input)
    {
        // try
        // {
        if (LocalPlayer == null)
        {
            // Assign mark randomly
            Random random = new Random();
            int randomInt = random.Next(0, 2);
            Mark randomMark = randomInt == 0 ? Mark.O : Mark.X;
        
            LocalPlayer = new Player(randomMark);
        }
        
        // if (input.Length != 2)
        //     throw new Exception("Invalid coordinate");
        //
        Coordinate parsedCoordiante = Coordinate.Parse(input.ToUpper());
        
        GameState.Board.PlaceMark(parsedCoordiante, LocalPlayer.AssignedMark);
        
        // catch (Exception e)
        // {
            // _consoleService.SystemMessage(e.Message);
            // Console.WriteLine(e);
        //     throw;
        // }
    }
    
 
    
    public void NewOfflineGame()
    {
        ChangeAppState(AppState.InOfflineGame);
        
        _consoleService.SystemMessage("New offline game started");
    }
    
    public async Task NewOnlineGame(string serverUri)
    {
        ChangeAppState(AppState.InOnlineGame);
        
        var playerClient = new PlayerClient();
        
        // Start the WebSocket connection in the background
        Task.Run(async () =>
        {
            try
            {
                await playerClient.ConnectToWebSocket(serverUri);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // throw; // we don't want to throw because we want to keep the app running
            }
        });
               
        Console.WriteLine("New online game started");
    }
    
    private void ChangeAppState(AppState newState)
    {
        AppState = newState;
        _consoleService.SystemMessage($"Navigated to {newState}");
    }
    
    public void CreateLocalPlayer(Mark mark)
    {
        LocalPlayer = new Player(mark);
    }
}