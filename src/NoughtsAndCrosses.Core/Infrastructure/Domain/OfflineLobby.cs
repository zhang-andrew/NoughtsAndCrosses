using NoughtsAndCrosses.Core.Domain;
using NoughtsAndCrosses.Core.Manager;

namespace NoughtsAndCrosses.Core.Infrastructure.Domain;

public class OfflineLobby : LobbyBase
{
    public OfflineLobby()
    {
    }

    public override Task StartGame()
    {
        if (LobbyState == LobbyState.ReadyToStart)
        {
            Game game = new Game(Players[0], Players[1]);
            
            LobbyState = LobbyState.GameInProgress;
            
            return Task.CompletedTask;
        }
        
        throw new Exception("Lobby is not ready to start a game. Current lobby state: " + LobbyState);
    }
}

