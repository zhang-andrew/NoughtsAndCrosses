// using NoughtsAndCrosses.Core.Service;

using System.Runtime.InteropServices.JavaScript;
using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain;

public class GameManager
{
    public GameScreen GameScreen { get; private set;} = GameScreen.Menu;
    public Player? LocalPlayer = null;
    
    private ConsoleService _consoleService;
    
    public BoardState? BoardState = null;


    public GameManager()
    {
        _consoleService = new ConsoleService();
        
        BoardState = new BoardState();
    }

    public async void Run()
    {
        _consoleService.SystemMessage(GameScreen, "Welcome to Noughts and Crosses");
        _consoleService.SystemMessage(GameScreen, "1 - New offline game, 2 - New online game, 3 - Join online game");

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
                    _consoleService.SystemMessage(GameScreen, "Exiting...");
                    break; // immediately exit the loop
                }
                
                HandleInput(input);
                
            }
            catch (Exception e)
            {
                _consoleService.UnhandledExceptionMessage(e);
            }
        }
    }

    public void HandleInput(string input)
    {
        switch (GameScreen)
        {
            case GameScreen.Menu:
                HandleMenuInput(input);
                break;
            case GameScreen.OfflineGame:
            case GameScreen.Lobby:
                HandleGameInput(input);
                break;
            default:
                _consoleService.SystemMessage(GameScreen, "Invalid application state.");
                break;
        }
    }
    
    private void HandleMenuInput(string input)
    {
        switch (input)
        {
            case MenuCommands.GoToOfflineGameScreen:
                NewOfflineGame();
                break;
            case MenuCommands.GoToLobbyScreen:
                NewOnlineGame("ws://localhost:5148/ws");
                break;
            case "3":
                // JoinOnlineGame("ws://localhost:5148/ws", "gameId");
                break;
        }
    }
    
    private void HandleGameInput(string input)
    {
        if (LocalPlayer == null)
        {
            // Assign mark randomly
            Random random = new Random();
            int randomInt = random.Next(0, 2);
            Mark randomMark = randomInt == 0 ? Mark.O : Mark.X;
        
            LocalPlayer = new Player(randomMark);
        }
        
        Coordinate parsedCoordiante = Coordinate.Parse(input.ToUpper());
        BoardState.Board.PlaceMark(parsedCoordiante, LocalPlayer.AssignedMark);
    }

    private void NewOfflineGame()
    {
        ChangeScreen(GameScreen.OfflineGame);
    }

    private async Task NewOnlineGame(string serverUri)
    {
        ChangeScreen(GameScreen.Lobby);
        
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
    
    public void ChangeScreen(GameScreen newMode)
    {
        string stateName;
        GameScreen = newMode;
        
        switch (GameScreen)
        {
            case GameScreen.OfflineGame:
                stateName = "Game (Offline)";
                break;
            default:
                stateName = GameScreen.ToString();
                break;
        }
        
        _consoleService.SystemMessage(GameScreen, $"Navigated to \"{stateName}\".");
    }
    
    public void CreateLocalPlayer(Mark mark)
    {
        LocalPlayer = new Player(mark);
    }
}