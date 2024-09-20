using FluentAssertions;
// using NoughtsAndCrosses.ConsoleApp.Domain;
using NoughtsAndCrosses.Core;
using NoughtsAndCrosses.Core.Constant;
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
        
        gameManager.ChangeScreen(GameScreen.OfflineGame);
        gameManager.CreateLocalPlayer(markType);
        
        // Act
        gameManager.HandleInput(input); // triggers the method that places the mark on the board
        Space affectedSpace = gameManager.BoardState.Board.GetSpace(input); 
        
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
        gameManager.ChangeScreen(GameScreen.OfflineGame);
        gameManager.CreateLocalPlayer(markType);
        
        // Act
        Action act = () => gameManager.HandleInput(input);
        
        // Assert
        // act.Should().Throw<Exception>().WithMessage("Invalid coordinate");

        act.Should().Throw<Exception>();
    }
    
 
    [Fact]
    public void Should_close_game_when_close_command_is_given()
    {
        // Arrange
        var gameManager = new GameManager();
        gameManager.ChangeScreen(GameScreen.Menu); // TODO: should take any screen
        
        // Act
        gameManager.HandleInput(GeneralCommand.CloseApplication);
        
        // Assert
        gameManager.ListeningForInputs.Should().BeFalse();

    }
    
    [Fact]
    public void Should_change_screens_with_goto_commands_in_menu()
    {
        // Arrange
        var gameManager = new GameManager();
        gameManager.ChangeScreen(GameScreen.Menu);
        
        // Act
        gameManager.HandleInput(MenuCommand.GoToLobbyScreen);
        // gameManager.HandleInput("2");
        // gameManager.HandleInput("3");
        
        // Assert
        gameManager.GameScreen.Should().Be(GameScreen.Lobby);
    }


    public void Should_accept_menu_commands_in_menu()
    {
    }
}