using NoughtsAndCrosses.Core.Domain.GameScreens;
using NoughtsAndCrosses.Core.Enum;

namespace NoughtsAndCrosses.Core.Domain;

public class GameManager
{
    public Board Board;
    private List<Player> _players = new() { };
    public Player ClientPlayer; // The player that is playing on the local machine
    public Player TurnPlayer;
    
    public GameManager()
    {
    }

    public void StartGame(Mark? clientAssignedMark = null)
    {
        Board = new Board();
        
        // First handle the client player
        if (System.Enum.TryParse(clientAssignedMark.ToString(), out Mark mark))
        {
            ClientPlayer = new Player(mark);
        } else
        {
            Random random = new Random();
            ClientPlayer = new Player(random.Next(0, 2) == 0 ? Mark.X : Mark.O);
        }
        _players.Add(ClientPlayer);
        
        // Then handle the other player
        Mark opponentMark = ClientPlayer.AssignedMark == Mark.X ? Mark.O : Mark.X;
        Player opponentPlayer = new Player(opponentMark);
        _players.Add(opponentPlayer);
        
        // Assign the X player to go first
        TurnPlayer = _players.First(p => p.AssignedMark == Mark.X); // X always goes first

        // Notify the client player of their mark
        ClientPlayer.NotifyPlayerMark();
    }

    public void HostGame()
    {
    }

    public void JoinGame()
    {
        ClientPlayer.NotifyPlayerMark();
    }

    public void NextTurn()
    {
        TurnPlayer = TurnPlayer == _players[0] ? _players[1] : _players[0];
    }

    public void HandleTurn()
    {
        // Broadcast messages
        foreach (var player in _players)
        {
            if (player == ClientPlayer)
            {
                player.IsTheirTurn = true;
                player.NotifyTurn();
            }
            else
            {
                player.IsTheirTurn = false;
                player.NotifyWait();
            }
        }
    }

}