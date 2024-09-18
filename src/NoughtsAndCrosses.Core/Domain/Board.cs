using NoughtsAndCrosses.Core.Constant;
using NoughtsAndCrosses.Core.Enum;

namespace NoughtsAndCrosses.Core.Domain;

public class Board
{
    public Space[] Spaces { get; } = new Space[9]
    {
        new Space(new Coordinate(FileLetter.A, 3)),
        new Space(new Coordinate(FileLetter.B, 3)),
        new Space(new Coordinate(FileLetter.C, 3)),
        new Space(new Coordinate(FileLetter.A, 2)),
        new Space(new Coordinate(FileLetter.B, 2)),
        new Space(new Coordinate(FileLetter.C, 2)),
        new Space(new Coordinate(FileLetter.A, 1)),
        new Space(new Coordinate(FileLetter.B, 1)),
        new Space(new Coordinate(FileLetter.C, 1))
    };

    public void ShowBoard()
    {
        for (int i = 0; i < Spaces.Length; i++)
        {
            
            if (i % 3 == 0)
            {
                string rankAsString = ((int)Spaces[i].Coordinate.Rank).ToString();
                Console.Write($"{rankAsString} ");
            }

            string mark = Spaces[i].Mark == Mark.Empty ? " " : Spaces[i].Mark.ToString();
            
            if ((i+1) % 3 == 0)
            {
                Console.WriteLine($"[{mark}]");
                // Console.WriteLine($"[{Spaces[i].Coordinate.Value}]");
            } else {
                Console.Write($"[{mark}]");
                // Console.Write($"[{Spaces[i].Coordinate.Value}]");
            }    
        }
        
        Console.WriteLine($"   {FileLetter.A}  {FileLetter.B}  {FileLetter.C}".PadLeft(3));
    }

    public void PlaceMark(Coordinate coordinate, Mark mark)
    {
        Space space = Spaces.First(s => s.Coordinate.Value == coordinate.Value);
        space.Mark = mark;
    }
    
    public void PlaceMarkRandomly(Mark mark)
    {
        Random random = new Random();
        int randomInt = random.Next(0, 9);
        var randomSpace = Spaces[randomInt];
        // PlaceMark(randomSpace, mark);
        randomSpace.Mark = mark;
    }
    
    public void PlaceBestMark(Mark mark) // for Computer Player
    {
        // Check for winning move
        
        // Stop player from winning
        
        // Best setup for winning move
        
        // Go for middle square if available
        
        // Go for corner square if available
        
        // Space bestSpace = GetBestSpace();
        // bestSpace.Mark = mark;

        throw new NotImplementedException();
    }

    public Space GetSpace(string coordinate)
    {
        Coordinate parsedCoordinate = Coordinate.Parse(coordinate);
        
        return Spaces.First(s => s.Coordinate.Value == parsedCoordinate.Value);
    }
    
    public bool HasWinner()
    {
        bool hasWinner = false;
        
        List<string[]> winningCombinations = new()
        {
            new string[]{ "A1", "B1", "C1" }, // horizontal - 1st row
            new string[]{ "A2", "B2", "C2" }, // horizontal - 2nd row
            new string[]{ "A3", "B3", "C3" }, // horizontal - 3rd row
            new string[]{ "A1", "A2", "A3" }, // vertical - 1st column
            new string[]{ "B1", "B2", "B3" }, // vertical - 2nd column
            new string[]{ "C1", "C2", "C3" }, // vertical - 3rd column
            new string[]{ "A1", "B2", "C3" }, // diagonal - top left to bottom right
            new string[]{ "A3", "B2", "C1" }  // diagonal - bottom left to top right
        };

        List<string> xMarks = new();
        List<string> oMarks = new();
        
        foreach (var space in Spaces)
        {
            if (space.Mark == Mark.X)
            {
                xMarks.Add(space.Coordinate.Value);
            }
            else if (space.Mark == Mark.O)
            {
                oMarks.Add(space.Coordinate.Value);
            }
        }

        for (int i = 0; i < winningCombinations.Count; i++)
        {
            string[] coordinates = winningCombinations[i];
            
            if (coordinates.All(coordinate => xMarks.Contains(coordinate)))
            {
                hasWinner = true;
                break;
            }
            else if (coordinates.All(s => oMarks.Contains(s)))
            {
                hasWinner = true;
                break;
            }
        }
        return hasWinner;
    }

    public bool HasDraw()
    {
        if (HasWinner())
            return false;
        
        bool fullBoard = Spaces.All(s => s.Mark != Mark.Empty);

        return fullBoard;
    }
}