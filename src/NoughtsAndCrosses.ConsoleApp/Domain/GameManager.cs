using NoughtsAndCrosses.Core.Domain;
using NoughtsAndCrosses.Core.Enum;

namespace NoughtsAndCrosses.ConsoleApp.Domain;

public class GameManager
{
    public Game Game { get; set; }

    public GameManager()
    {
        Game = new Game();
    }
    
    public void NewGame()
    {
        Game = new Game();
    }

    public string ConsoleInput(string input)
    {
        if (input.Length != 2)
            throw new Exception("Invalid coordinate");
        
        input = input.ToUpper();

        try
        {
            FileLetter fileLetter = System.Enum.Parse<FileLetter>(input[0].ToString());
            RankNumber rankNumber = System.Enum.Parse<RankNumber>(input[1].ToString());
            
            Game.Board.PlaceMark(new Coordinate(fileLetter, rankNumber), Mark.X);
            
            return $"{fileLetter}{(int)rankNumber}";
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