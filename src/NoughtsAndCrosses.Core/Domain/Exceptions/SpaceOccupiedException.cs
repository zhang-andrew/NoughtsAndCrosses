namespace NoughtsAndCrosses.Core.Domain.Exceptions;

public class SpaceOccupiedException : Exception
{
    public SpaceOccupiedException() : base("Space is already occupied.") { }
}
