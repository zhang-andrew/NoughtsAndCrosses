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
    public void Should_place_a_mark_if_coordinate_input_is_valid(Mark markType, string input)
    {
        // Arrange
        var gameManager = new AppManager();
        
        gameManager.ChangeScreen(GameScreen.InGame);
        gameManager.CreateLocalPlayer(markType);
        
        // Act
        gameManager.HandleInput(input); // triggers the method that places the mark on the board
        Space affectedSpace = gameManager.Board.GetSpace(input); 
        
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
        var gameManager = new AppManager();
        gameManager.ChangeScreen(GameScreen.InGame);
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
        var gameManager = new AppManager();
        gameManager.ChangeScreen(GameScreen.Menu); // TODO: should take any screen
        
        // Act
        gameManager.HandleInput(GeneralCommand.CloseApplication);
        
        // Assert
        gameManager.IsListeningForInputs.Should().BeFalse();

    }
    
    [Theory]
    [InlineData(GameScreen.InGame)]
    [InlineData(GameScreen.HostGame)]
    public void Should_go_back_to_menu_when_back_command_is_given(GameScreen gameScreen)
    {
        // Arrange
        var gameManager = new AppManager();
        gameManager.ChangeScreen(gameScreen);
        
        // Act
        gameManager.HandleInput(GeneralCommand.Back);
        
        // Assert
        gameManager.CurrentScreen.Should().Be(GameScreen.Menu);
    }
    
    [Fact]
    public void Should_change_screens_with_goto_commands_in_menu()
    {
        // Arrange
        var gameManager = new AppManager();
        gameManager.ChangeScreen(GameScreen.Menu);
        
        // Act
        gameManager.HandleInput(MenuCommand.GoToHostGame);
        // gameManager.HandleInput("2");
        // gameManager.HandleInput("3");
        
        // Assert
        gameManager.CurrentScreen.Should().Be(GameScreen.HostGame);
    }

    [Fact]
    public void Should_log_invalid_command_if_given()
    {
        // Arrange
        var gameManager = new AppManager();
        gameManager.ChangeScreen(GameScreen.Menu);
        
        // Act
        bool worked = gameManager.Screens[gameManager.CurrentScreen].HandleInput("ASDF");
        
        // Assert
        worked.Should().Be(false);
        // _consoleService.SystemMessage(GameScreen.Menu, "Invalid command.").Should().BeTrue();
    }
    
    [Fact]
    public void Should_restart_game_when_restart_command_is_given()
    {
        // Arrange
        var gameManager = new AppManager();
        gameManager.ChangeScreen(GameScreen.InGame);
        gameManager.Screens[gameManager.CurrentScreen].HandleInput("a1");
        gameManager.Screens[gameManager.CurrentScreen].HandleInput("b1");
        gameManager.Screens[gameManager.CurrentScreen].HandleInput("c1");
        
        gameManager.Board.GetWinner().Should().NotBe(Mark.Empty);
        gameManager.Board.HasWinner().Should().BeTrue();
        
        // Act
        gameManager.HandleInput("restart");
        
        // Assert
        gameManager.CurrentScreen.Should().Be(GameScreen.InGame);
        gameManager.Board.GetWinner().Should().Be(Mark.Empty);
        gameManager.Board.HasWinner().Should().BeFalse();
    }
}