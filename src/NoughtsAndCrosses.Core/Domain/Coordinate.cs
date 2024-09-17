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
}