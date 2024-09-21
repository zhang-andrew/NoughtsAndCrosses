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
                throw new Exception("Space is already occupied");
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