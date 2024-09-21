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
    public Player? LocalPlayer;
    public Board? Board;
    public bool IsListeningForInputs;
    
    private ConsoleService _consoleService;
    public Dictionary<GameScreen, IScreen> Screens;
    
    public GameManager()
    {
        _consoleService = new ConsoleService();
        
        Board = new Board();
        
        Screens = new ()
        {
            { GameScreen.Menu, new MenuScreen(this) },
            { GameScreen.HostGame, new HostGameScreen(this) },
            { GameScreen.JoinGame, new JoinGameScreen(this) },
            { GameScreen.InGame, new InGameScreen(this) },
        };
    }

    public async void Run()
    {
        CurrentScreen = GameScreen.Menu;
        Screens[CurrentScreen].OnEntry();
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
        // Handle close command
        if (input == GeneralCommand.CloseApplication)
        {
            _consoleService.SystemMessage(CurrentScreen, "Exiting...");
            IsListeningForInputs = false;
            return;
        }
        
        // Handle back command
        if (CurrentScreen != GameScreen.Menu)
        {
            if (input == GeneralCommand.Back)
            {
                ChangeScreen(GameScreen.Menu);
                return;
            }
        }
        
        // Handle screen-specific commands
        bool wasHandled = Screens[CurrentScreen].HandleInput(input);
        
        // Handle invalid command
        if (!wasHandled)
            _consoleService.SystemMessage(CurrentScreen, "Invalid command.");
    }
    
    public void ChangeScreen(GameScreen newMode)
    {
        Screens[CurrentScreen].OnExit();
        
        CurrentScreen = newMode;
        _consoleService.SystemMessage(CurrentScreen, $"Navigated to \"{CurrentScreen}\".");
        
        Screens[CurrentScreen].OnEntry();
    }
    
    public void CreateLocalPlayer(Mark mark)
    {
        LocalPlayer = new Player(mark);
    }
}