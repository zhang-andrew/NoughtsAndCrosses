using NoughtsAndCrosses.Core.Enum;

namespace NoughtsAndCrosses.Core.Domain;

public class Player
{
    public Guid Id { get; } = Guid.NewGuid();
    public Mark AssignedMark { get; set; }

    private bool IsComputer = false;

    public Player(Mark assignedMark, bool isComputer = false)
    {
        IsComputer = isComputer;
        AssignedMark = assignedMark;
    }

    public void AssignMark(Mark mark)
    {
        AssignedMark = mark;
    }
}