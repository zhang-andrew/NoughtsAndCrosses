// using NoughtsAndCrosses.Core.Service;

using System.Runtime.InteropServices.JavaScript;
using NoughtsAndCrosses.Core.Constant;
using NoughtsAndCrosses.Core.Domain.GameScreens;
using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Infrastructure;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain;

public class AppManager
{
    // Singleton implementation ()
    private static readonly AppManager _instance = new AppManager();
    public static AppManager Instance
    {
        get { return _instance; }
    }
    
    public AppScreen CurrentScreen { get; private set;}
    public GameManager GameManager = GameManager.Instance;

    public bool IsListeningForInputs;
    
    private ConsoleService _consoleService;
    public Dictionary<AppScreen, IScreen> Screens;
    
    private AppManager()
    {
        _consoleService = new ConsoleService();
        
        Screens = new ()
        {
            { AppScreen.Menu, new MenuScreen(this) },
            { AppScreen.HostGame, new HostGameScreen(this) },
            { AppScreen.JoinGame, new JoinGameScreen(this) },
            { AppScreen.InGame, new InGameScreen(this) },
            { AppScreen.PreGame, new PreGameScreen(this) },
            { AppScreen.PostGame, new PostGameScreen(this) }
        };
    }

    public async void Run()
    {
        CurrentScreen = AppScreen.Menu;
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
                
                WriteInput(input);
                
                if (IsListeningForInputs == false)
                {
                    break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unhandled exception occurred. Exiting...");
                throw; // Rethrow the exception
            }
        }
    }

    public void WriteInput(string input)
    {
        // Handle close command
        if (input == GeneralCommand.CloseApplication)
        {
            _consoleService.SystemMessage( "Exiting...");
            IsListeningForInputs = false;
            return;
        }
        
        // Handle back command
        if (CurrentScreen != AppScreen.Menu 
            || CurrentScreen != AppScreen.InGame 
            || CurrentScreen != AppScreen.PostGame)
        {
            if (input == GeneralCommand.Back)
            {
                ChangeScreen(AppScreen.Menu);
                return;
            }
        }
        
        // Handle screen-specific commands
        bool inputResult = Screens[CurrentScreen].HandleInput(input);
        if (!inputResult)
        {
            _consoleService.SystemMessage( "Invalid input.");
        }
    }
    
    public void ChangeScreen(AppScreen newMode)
    {
        Screens[CurrentScreen].OnExit();
        _consoleService.SystemMessage($"Exited \"{CurrentScreen}\" screen.");        
        Console.WriteLine("");
        
        CurrentScreen = newMode;
        
        _consoleService.SystemMessage($"Entered \"{CurrentScreen}\" screen.");
        Screens[CurrentScreen].OnEntry();
    }
    
}