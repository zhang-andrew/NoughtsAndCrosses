// using NoughtsAndCrosses.ConsoleApp.Domain;
using NoughtsAndCrosses.Core.Domain;
using NoughtsAndCrosses.Core.Enum;

namespace NoughtsAndCrosses.Core.Service;

public class ConsoleService
{
    public void SystemMessage(string message)
    {
        Console.WriteLine($"[System] {message}");
    }
    
    /* Handled/Known exception messages */
    public void HandledExceptionMessage(Exception e, string solutionMessage = "")
    {
        Console.WriteLine($"[Error] {e.Message}. {solutionMessage}");
    }
}