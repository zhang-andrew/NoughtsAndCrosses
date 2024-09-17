using NoughtsAndCrosses.Core.Enum;

namespace NoughtsAndCrosses.ConsoleApp.Domain;

public class Player
{
    public Mark Mark { get; }

    public Player(Mark mark)
    {
        Mark = mark;
    }
}