using NoughtsAndCrosses.Core.Enum;

namespace NoughtsAndCrosses.Core.Domain;

/*
 * the horizontal lines of the board are called ranks
 * the vertical lines of the board are called files
 */
public class Coordinate
{
    public string Value => $"{File}{(int)Rank}";
    public FileLetter File { get; }
    public RankNumber Rank { get; }
    public Coordinate(FileLetter fileLetter, RankNumber rankNumber)
    {
        File = fileLetter;
        Rank = rankNumber;
    }
}