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
        var appManager = AppManager.Instance;
        appManager.ChangeScreen(AppScreen.InGame);
        
        
        
        // Act
        appManager.HandleInput(input); // triggers the method that places the mark on the board
        Space affectedSpace = appManager.GameManager.Game.GetSpace(input); 
        
        // Assert
        affectedSpace.Mark.Should().Be(appManager.GameManager.ClientPlayer.AssignedMark);
    }
    
    [Theory]
    [InlineData(Mark.X, "aa")]
    [InlineData(Mark.X, "HelloWorld")]
    [InlineData(Mark.X, "11")]
    public void Should_throw_exception_if_coordinate_input_is_invalid(Mark markType, string input)
    {
        // Arrange
        var appManager = AppManager.Instance;
        appManager.ChangeScreen(AppScreen.InGame);
        
        var gameManager = GameManager.Instance;
        gameManager.NewGame();
        
        // Act
        Action act = () => appManager.HandleInput(input);
        
        // Assert
        // act.Should().Throw<Exception>().WithMessage("Invalid coordinate");

        act.Should().Throw<Exception>();
    }
    
 
    [Fact]
    public void Should_close_game_when_close_command_is_given()
    {
        // Arrange
        var gameManager = AppManager.Instance;
        gameManager.ChangeScreen(AppScreen.Menu); // TODO: should take any screen
        
        // Act
        gameManager.HandleInput(GeneralCommand.CloseApplication);
        
        // Assert
        gameManager.IsListeningForInputs.Should().BeFalse();

    }
    
    [Theory]
    [InlineData(AppScreen.InGame)]
    [InlineData(AppScreen.HostGame)]
    public void Should_go_back_to_menu_when_back_command_is_given(AppScreen appScreen)
    {
        // Arrange
        var gameManager = AppManager.Instance;
        gameManager.ChangeScreen(appScreen);
        
        // Act
        gameManager.HandleInput(GeneralCommand.Back);
        
        // Assert
        gameManager.CurrentScreen.Should().Be(AppScreen.Menu);
    }
    
    [Fact]
    public void Should_change_screens_with_goto_commands_in_menu()
    {
        // Arrange
        var gameManager = AppManager.Instance;
        gameManager.ChangeScreen(AppScreen.Menu);
        
        // Act
        gameManager.HandleInput(MenuCommand.GoToHostGame);
        // gameManager.HandleInput("2");
        // gameManager.HandleInput("3");
        
        // Assert
        gameManager.CurrentScreen.Should().Be(AppScreen.HostGame);
    }

    [Fact]
    public void Should_log_invalid_command_if_given()
    {
        // Arrange
        var gameManager = AppManager.Instance;
        gameManager.ChangeScreen(AppScreen.Menu);
        
        // Act
        bool worked = gameManager.Screens[gameManager.CurrentScreen].HandleInput("ASDF");
        
        // Assert
        worked.Should().Be(false);
        // _consoleService.SystemMessage(GameScreen.Menu, "Invalid command.").Should().BeTrue();
    }
    
}