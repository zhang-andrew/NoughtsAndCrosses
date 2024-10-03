using NoughtsAndCrosses.Core.Domain;

namespace NoughtsAndCrosses.WebSocketServer.Domain;


public class Lobby
{
    public Guid Id { get; } = Guid.NewGuid();
    public List<Player> Players { get; } = new List<Player>();
    public Game Game { get; set; }
    
    public void NewGame(bool isOnline)
    {
        if (isOnline)
        {
            Game = new Game(true);
        }
        else
        {
            Game = new Game(false);
        }

    }
    
    public void UpdateGame(Game game)
    {
        Game = game;

        if (game.IsOnline)
        {
            // send game to all players
        }
    }
}