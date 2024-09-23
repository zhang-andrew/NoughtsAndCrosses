using NoughtsAndCrosses.Core.Constant;
using NoughtsAndCrosses.Core.Domain.Exceptions;
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
            throw new InvalidCoordinateException();
        
        value = value.ToUpper();
        
        int? rankNumber = Int32.TryParse(value[1].ToString(), out int parsedSecondChar) ? parsedSecondChar : null;
        string fileLetter = value[0].ToString();

        // TODO: improve this 
        // If the above Int32.TryParse fails for the 2nd char, it will return null, so we try to parse the 1st char: 
        if (rankNumber == null)
        {
            rankNumber = Int32.TryParse(value[0].ToString(), out int parsedFirstChar) ? parsedFirstChar : null;
            fileLetter = value[1].ToString();
        }

        string[] validFileLetters = new string[3] {FileLetter.A, FileLetter.B, FileLetter.C};
        int?[] validRankNumbers = new int?[3] {1, 2, 3};
        
        if (!validFileLetters.Contains(fileLetter))
            throw new InvalidCoordinateException();
        
        if (!validRankNumbers.Contains(rankNumber))
            throw new InvalidCoordinateException();
        
        // Get the FileLetter and RankNumber from the string
        return new Coordinate(fileLetter, (int)rankNumber);
    }
}