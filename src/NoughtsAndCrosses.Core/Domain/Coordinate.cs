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
    public Coordinate(FileLetter fileLetter, RankNumber rankNumber)
    {
        File = fileLetter.ToString();
        Rank = (int)rankNumber;
    }
    
    public static Coordinate Parse(string value)
    {
        if (value.Length != 2)
            throw new Exception("Invalid coordinate");
        
        value = value.ToUpper();

        FileLetter fileLetter = System.Enum.Parse<FileLetter>(value[0].ToString());
        RankNumber rankNumber = System.Enum.Parse<RankNumber>(value[1].ToString());
        
        return new Coordinate(fileLetter, rankNumber);
    }
}