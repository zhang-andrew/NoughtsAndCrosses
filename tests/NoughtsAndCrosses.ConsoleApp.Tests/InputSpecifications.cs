using FluentAssertions;
// using NoughtsAndCrosses.ConsoleApp.Domain;
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
    public void Should_place_a_mark_if_coordinate_input_is_valid(Mark markType, string input)
    {
        // Arrange
        var gameManager = new GameManager();
        gameManager.CreateLocalPlayer(markType);
        
        // Act
        gameManager.HandleGameInput(input); // triggers the method that places the mark on the board
        Space affectedSpace = gameManager.GameState.Board.GetSpace(input); 
        
        // Assert
        affectedSpace.Mark.Should().Be(gameManager.LocalPlayer.AssignedMark);
    }
    
    [Theory]
    [InlineData(Mark.X, "aa")]
    [InlineData(Mark.X, "HelloWorld")]
    [InlineData(Mark.X, "11")]
    public void Should_throw_exception_if_coordinate_input_is_invalid(Mark markType, string input)
    {
        // Arrange
        
        var gameManager = new GameManager();
        gameManager.CreateLocalPlayer(markType);
        
        // Act
        Action act = () => gameManager.HandleGameInput(input);
        
        // Assert
        // act.Should().Throw<Exception>().WithMessage("Invalid coordinate");

        act.Should().Throw<Exception>();
    }
    
    
}