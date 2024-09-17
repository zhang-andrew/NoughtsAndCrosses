using FluentAssertions;
using NoughtsAndCrosses.ConsoleApp.Domain;
using NoughtsAndCrosses.Core;
using NoughtsAndCrosses.Core.Domain;
using NoughtsAndCrosses.Core.Enum;

namespace NoughtsAndCrosses.ConsoleApp.Tests;

public class InputSpecifications
{
    [Fact]
    public void Should_place_a_mark_if_valid_coordinate()
    {
        var gameManager = new GameManager();
        
        // Arrange
        // var game = gameManager.NewGame();
        // var player = new Player(Mark.X);
        // var opponent = new Player(Mark.O);
        // game.AddPlayer(player);
        // game.AddPlayer(opponent);

        

        // Act
        var result = gameManager.ConsoleInput("a1");

        // Assert
        // Assert.Equal(opponent, game.CurrentPlayer);
        
        result.Should().Be("A1");
        
        Space affectedSpace = gameManager.Game.Board.GetSpace(new Coordinate(FileLetter.A, RankNumber.One)); 
        
        affectedSpace.Mark.Should().Be(Mark.X);

    }
}