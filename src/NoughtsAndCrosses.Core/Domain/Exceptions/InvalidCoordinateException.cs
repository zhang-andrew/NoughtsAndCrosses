namespace NoughtsAndCrosses.Core.Domain.Exceptions;

public class InvalidCoordinateException : Exception
{
    public InvalidCoordinateException() : base("Invalid coordinate.") { }
}