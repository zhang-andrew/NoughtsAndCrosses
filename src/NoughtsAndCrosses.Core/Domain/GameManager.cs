using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain;

public class GameManager
{
    public Game Game { get; set; }
    public Player[] Players { get; set; } = new Player[2];
    public Player PlayerWithTurn { get; set; }

    public ConsoleService ConsoleService;

    public GameManager()
    {
        Game = new Game();
    }
    
    public void NewGame()
    {
        Game = new Game();
    }

    public void ConsoleInput(Player player, string input)
    {
        if (input.Length != 2)
            throw new Exception("Invalid coordinate");
        
        input = input.ToUpper();

        try
        {
            Coordinate parsedCoordiante = Coordinate.Parse(input);
            Game.Board.PlaceMark(parsedCoordiante, player.Mark);
        }
        catch (Exception e)
        {
            throw new Exception("Invalid coordinate");
        }
    }
    
    
}


public class Game
{
    public Board Board { get; }
    
    public Game()
    {
        Board = new Board();       
    }
}