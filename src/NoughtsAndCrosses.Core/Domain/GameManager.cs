// using NoughtsAndCrosses.Core.Service;

using System.Runtime.InteropServices.JavaScript;
using NoughtsAndCrosses.Core.Constant;
using NoughtsAndCrosses.Core.Domain.GameScreens;
using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Infrastructure;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain;

public class GameManager
{
    public GameScreen CurrentScreen { get; private set;}

    public Player? LocalPlayer = null;
    
    public BoardState? BoardState = null;
    
    private ConsoleService _consoleService;

    public bool IsListeningForInputs;

    private Dictionary<GameScreen, IScreen> _screens;
    
    
    public GameManager()
    {
        _consoleService = new ConsoleService();
        
        BoardState = new BoardState();
        
        _screens = new ()
        {
            {GameScreen.Menu, new MenuScreen(this)},
            {GameScreen.OfflineGame, new OfflineGameScreen(this)}
        };
    }

    public async void Run()
    {
        CurrentScreen = GameScreen.Menu;
        _screens[CurrentScreen].OnEntry();
        await ListenForInputs();
    }
    
    private async Task ListenForInputs()
    {
        IsListeningForInputs = true;
        
        while (IsListeningForInputs)
        {
            try
            {
                string? input = Console.ReadLine();
                
                HandleInput(input);
                
                if (IsListeningForInputs == false)
                {
                    break;
                }
            }
            catch (Exception e)
            {
                _consoleService.UnhandledExceptionMessage(e);
            }
        }
    }

    public void HandleInput(string input)
    {
        if (input == GeneralCommand.CloseApplication)
        {
            _consoleService.SystemMessage(CurrentScreen, "Exiting...");
            IsListeningForInputs = false;
            return;
        }
        
        if (CurrentScreen != GameScreen.Menu)
        {
            if (input == GeneralCommand.Back)
            {
                ChangeScreen(GameScreen.Menu);
                return;
            }
        }
        
        _screens[CurrentScreen].HandleInputs(input);
        
        /* Commented out: Refactoring in progress
         * Replacing switch statement with IScreen.HandleInputs method
         * The correct handling is based on the current screen
         */
        // switch (CurrentScreen)
        // {
        //     case GameScreen.Menu:
        //         HandleMenuInput(input);
        //         break;
        //     case GameScreen.OfflineGame:
        //     case GameScreen.OnlineGame:
        //         HandleGameInput(input);
        //         break;
        //     case GameScreen.HostGame:
        //         break;
        //     default:
        //         _consoleService.SystemMessage(CurrentScreen, "Invalid application state.");
        //         break;
        // }
    }
    
    private void HandleMenuInput(string input)
    {
        switch (input)
        {
            case MenuCommand.GoToOfflineGameScreen:
                NewOfflineGame();
                break;
            case MenuCommand.GoToHostScreen:
                NewOnlineGame("ws://localhost:5148/ws");
                break;
            case MenuCommand.GoToJoinOnlineGame:
                JoinOnlineGame("ws://localhost:5148/ws", "gameId");
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
        ChangeScreen(GameScreen.HostGame);
        
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
    
    
    private void JoinOnlineGame(string wsLocalhostWs, string gameid)
    {
        ChangeScreen(GameScreen.HostGame);

        throw new NotImplementedException();
    }

    
    public void ChangeScreen(GameScreen newMode)
    {
        _screens[CurrentScreen].OnExit();
        
        CurrentScreen = newMode;
        _consoleService.SystemMessage(CurrentScreen, $"Navigated to \"{CurrentScreen}\".");
        _screens[CurrentScreen].OnEntry();
    }
    
    public void CreateLocalPlayer(Mark mark)
    {
        LocalPlayer = new Player(mark);
    }
}