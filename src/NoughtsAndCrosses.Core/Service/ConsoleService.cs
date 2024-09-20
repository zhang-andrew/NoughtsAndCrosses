// using NoughtsAndCrosses.ConsoleApp.Domain;
using NoughtsAndCrosses.Core.Domain;
using NoughtsAndCrosses.Core.Enum;

namespace NoughtsAndCrosses.Core.Service;

public class ConsoleService
{
    public void SystemMessage(GameScreen gameScreen, string message)
    {
        Console.WriteLine($"[System][{gameScreen}] {message}");
    }
    
    /* Unhandled exception messages */
    public void UnhandledExceptionMessage(Exception e)
    {
        Console.WriteLine($"[Exception] {e}");
    }
    
    /* Handled/Known exception messages */
    public void HandledExceptionMessage(Exception e, string solutionMessage)
    {
        Console.WriteLine($"[Error] {e.Message}. {solutionMessage}");
    }
}