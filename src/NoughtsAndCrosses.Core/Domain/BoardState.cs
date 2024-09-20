namespace NoughtsAndCrosses.Core.Domain;

public class BoardState
{
    public Board Board { get; }
    
    public BoardState()
    {
        Board = new Board();       
    }
}