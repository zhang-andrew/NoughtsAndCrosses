using FluentAssertions;
using NoughtsAndCrosses.ConsoleApp.Domain;
using NoughtsAndCrosses.Core;
using NoughtsAndCrosses.Core.Domain;
using NoughtsAndCrosses.Core.Enum;

namespace NoughtsAndCrosses.ConsoleApp.Tests;

public class InputSpecifications
{
    [Theory]
    [InlineData(Mark.X, "a1")]
    [InlineData(Mark.X, "A2")]
    [InlineData(Mark.O, "b3")]
    [InlineData(Mark.O, "B3")]
    public void Should_place_a_mark_if_valid_coordinate_input_is_given(Mark markType, string input)
    {
        // Arrange
        var gameManager = new GameManager();
        var player = new Player(markType);
        
        // Act
        gameManager.ConsoleInput(player, input); // triggers the method that places the mark on the board
        Space affectedSpace = gameManager.Game.Board.GetSpace(input); 
        
        // Assert
        affectedSpace.Mark.Should().Be(player.Mark);

    }
}