using NoughtsAndCrosses.Core.Domain.Exceptions;
using NoughtsAndCrosses.Core.Enum;

namespace NoughtsAndCrosses.Core.Domain;

public class Space
{
    private Mark _mark;
    
    public Coordinate Coordinate { get; }

    public Space(Coordinate coordinate)
    {
        Coordinate = coordinate;
        _mark = Mark.Empty;
    }

    public Mark Mark
    {
        get { return _mark; }
        set
        {
            if (_mark != Mark.Empty)
                throw new SpaceOccupiedException();
            else
            {
                _mark = value;
            }
        }
    }

    public void Clear()
    {
        _mark = Mark.Empty;
    }
}