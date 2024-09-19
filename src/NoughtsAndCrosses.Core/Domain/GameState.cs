namespace NoughtsAndCrosses.Core.Domain;

public class GameState
{
    public Board Board { get; }
    
    public GameState()
    {
        Board = new Board();       
    }
}