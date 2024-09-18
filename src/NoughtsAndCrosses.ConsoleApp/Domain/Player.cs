using NoughtsAndCrosses.Core.Enum;

namespace NoughtsAndCrosses.ConsoleApp.Domain;

public class Player
{
    public Mark Mark { get; }

    private bool IsComputer = false;

    public Player(Mark mark, bool isComputer = false)
    {
        Mark = mark;
        IsComputer = isComputer;
    }
}