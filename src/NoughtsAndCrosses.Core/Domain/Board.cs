using NoughtsAndCrosses.Core.Enum;

namespace NoughtsAndCrosses.Core.Domain;

public class Board
{
    public Space[] Spaces { get; } = new Space[9]
    {
        new Space(new Coordinate(FileLetter.A, RankNumber.Three)),
        new Space(new Coordinate(FileLetter.B, RankNumber.Three)),
        new Space(new Coordinate(FileLetter.C, RankNumber.Three)),
        new Space(new Coordinate(FileLetter.A, RankNumber.Two)),
        new Space(new Coordinate(FileLetter.B, RankNumber.Two)),
        new Space(new Coordinate(FileLetter.C, RankNumber.Two)),
        new Space(new Coordinate(FileLetter.A, RankNumber.One)),
        new Space(new Coordinate(FileLetter.B, RankNumber.One)),
        new Space(new Coordinate(FileLetter.C, RankNumber.One))
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
}