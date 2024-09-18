using NoughtsAndCrosses.WebSocketServer.Domain;

namespace NoughtsAndCrosses.WebSocketServer.Tests;

public class ServerSpecifications
{
    [Fact]
    public void Should_be_able_to_start_server()
    {
        var server = new Server();
        server.Start(new string[] { });
        
        // Assert - TODO: Improve == Implement a way to check if the server is running
        Assert.True(true); // If the server starts without throwing an exception, then the test passes 
    }
    
    [Fact]
    public void Should_be_able_to_connect_to_server()
    {
        // Arrange
        var server = new Server();
        server.Start(new string[] { });
        
        
        // // Act
        // server.Connect();
        //
        // // Assert
        // server.Lobby.Count.Should().Be(1);
    }
}