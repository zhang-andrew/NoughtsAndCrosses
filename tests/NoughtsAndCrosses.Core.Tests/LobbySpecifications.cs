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
using NoughtsAndCrosses.Core.Infrastructure.Domain;
using NoughtsAndCrosses.Core.Manager;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core;

public class LobbySpecifications : IDisposable
{
    public void Dispose()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.ResetGame();
    }
    
    [Fact]
    public async void Create_offline_lobby_should_return_lobby()
    {
        // Arrange
        var gm = GameManager.Instance;
        
        // Act
        Player hostPlayer = new Player(); 
        LobbyBase lobby = await gm.CreateLobby(hostPlayer, false);
        
        // Assert
        lobby.Should().NotBeNull();
        lobby.LobbyState.Should().Be(LobbyState.WaitingForOpponent);
        lobby.Players.Should().Contain(hostPlayer);
        gm.LocalClient.IsConnected.Should().BeFalse();
        
    }
    
    [Fact]
    public async void Create_online_lobby_should_return_lobby()
    {
        // Arrange
        var gm = GameManager.Instance;
        gm.UseMockLocalClient();
        
        // Act
        Player hostPlayer = new Player(); 
        LobbyBase lobby = await gm.CreateLobby(hostPlayer, true);
        
        // Assert
        lobby.Should().NotBeNull();
        lobby.LobbyState.Should().Be(LobbyState.WaitingForOpponent);
        lobby.Players.Should().Contain(hostPlayer);
        gm.LocalClient.IsConnected.Should().BeTrue();
    }
    
    // [Fact]
    
}
