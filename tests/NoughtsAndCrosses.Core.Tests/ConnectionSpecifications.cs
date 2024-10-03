using FluentAssertions;
using NoughtsAndCrosses.Core.Domain;
using NoughtsAndCrosses.Core.Enum;
using System;
using System.Runtime.CompilerServices;
// using NoughtsAndCrosses.ConsoleApp;
// using NoughtsAndCrosses.ConsoleApp.Domain;
using NoughtsAndCrosses.Core.Constant;
using NoughtsAndCrosses.Core.Domain.Exceptions;
using NoughtsAndCrosses.Core.Infrastructure;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core;

public class ConnectionSpecifications : IDisposable
{
    public void Dispose()
    {
        // 
    }
    

    [Fact]
    public async void Should_Join_Lobby()
    {
        // Arrange
        var lobbyService = new LocalClient();
        await lobbyService.Connect();
        Guid lobbyId = await lobbyService.CreateLobby();
        var player = new Player(Mark.X);

        // Act
        await lobbyService.AddWaitingPlayer(lobbyId, player);

        // Assert
        var game = await lobbyService.GetGameFromLobby(lobbyId);
        game.Players.Should().Contain(player);
    }

}
