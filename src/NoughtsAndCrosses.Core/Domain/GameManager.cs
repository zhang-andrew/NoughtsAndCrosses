// using NoughtsAndCrosses.Core.Service;

using System.Runtime.InteropServices.JavaScript;
using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain;

public class GameManager
{
    private AppState _appState = AppState.Menu;
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
        _consoleService.SystemMessage(_appState, "Welcome to Noughts and Crosses");
        _consoleService.SystemMessage(_appState, "1 - New offline game, 2 - New online game, 3 - Join online game");

        await ListenForInputs();
    }

    private async Task ListenForInputs()
    {
        while (true)
        {
            try
            {
                string? input = Console.ReadLine();
            
                if (input == "exit")
                {
                    _consoleService.SystemMessage(_appState, "Exiting...");
                    break; // immediately exit the loop
                }
            
                switch (_appState)
                {
                    case AppState.Menu:
                        HandleMenuInput(input);
                        break;
                    case AppState.OfflineGame:
                    case AppState.Lobby:
                        HandleGameInput(input);
                        break;
                    default:
                        _consoleService.SystemMessage(_appState, "Invalid application state.");
                        break;
                }
            }
            catch (Exception e)
            {
                _consoleService.UnhandledExceptionMessage(e);
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
            // _consoleService.SystemMessage(AppState, e.Message);
            // Console.WriteLine(e);
        //     throw;
        // }
    }
    
 
    
    public void NewOfflineGame()
    {
        ChangeAppState(AppState.OfflineGame);
        
    }
    
    public async Task NewOnlineGame(string serverUri)
    {
        ChangeAppState(AppState.Lobby);
        
        var playerClient = new PlayerClient(_consoleService);
        
        // Start the WebSocket connection in the background
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
    }
    
    private void ChangeAppState(AppState newState)
    {
        string stateName;
        _appState = newState;
        
        switch (_appState)
        {
            case AppState.OfflineGame:
                stateName = "Game (Offline)";
                break;
            default:
                stateName = _appState.ToString();
                break;
        }
        
        _consoleService.SystemMessage(_appState, $"Navigated to \"{stateName}\".");
    }
    
    public void CreateLocalPlayer(Mark mark)
    {
        LocalPlayer = new Player(mark);
    }
}