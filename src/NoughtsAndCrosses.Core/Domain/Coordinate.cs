using NoughtsAndCrosses.Core.Constant;
using NoughtsAndCrosses.Core.Enum;

namespace NoughtsAndCrosses.Core.Domain;

/*
 * the horizontal lines of the board are called ranks
 * the vertical lines of the board are called files
 */
public class Coordinate
{
    public string Value => $"{File}{Rank}";
    public string File { get; }
    public int Rank { get; }
    public Coordinate(string file, int rank)
    {
        File = file;
        Rank = rank;
    }
    
    public static Coordinate Parse(string value)
    {
        if (value.Length != 2)
            throw new Exception("Invalid coordinate");
        
        value = value.ToUpper();
        
        string[] validFileLetters = new string[3] {FileLetter.A, FileLetter.B, FileLetter.C};
        int[] validRankNumbers = new int[3] {1, 2, 3};

        try
        {
            string fileLetter = value[0].ToString();
            int rankNumber = Int32.Parse(value[1].ToString());
            
            if (!validFileLetters.Contains(fileLetter))
                throw new Exception("Invalid coordinate (fileLetter)");
            
            if (!validRankNumbers.Contains(rankNumber))
                throw new Exception("Invalid coordinate (rankNumber)");
            
            // Get the FileLetter and RankNumber from the string
            return new Coordinate(fileLetter, rankNumber);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Invalid coordinate");
        }
    }
}