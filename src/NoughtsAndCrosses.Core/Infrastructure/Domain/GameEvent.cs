using NoughtsAndCrosses.Core.Domain;

namespace NoughtsAndCrosses.Core.Infrastructure.Domain;

public class GameEvent
{
    public GameEventType GameEventType { get; set; }
    public OfflineLobby OfflineLobby { get; set; }
    public Coordinate Coordinate { get; set; }
    public Player PlayerWhoMadeMove { get; set; }
    public Player PlayerWithNextTurn { get; set; }
    public Game Game { get; set; }
}

public enum GameEventType
{
    MoveMade,
    GameWon,
    GameDraw,
    GameStarted,
    GameEnded,
}