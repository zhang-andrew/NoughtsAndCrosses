// using NoughtsAndCrosses.ConsoleApp.Domain;
using NoughtsAndCrosses.Core.Domain;

namespace NoughtsAndCrosses.Core.Service;

public class ConsoleService
{
    public void SystemMessage(string message)
    {
        Console.WriteLine($"[System] {message}");
    }
}