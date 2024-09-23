using FluentAssertions;
using NoughtsAndCrosses.Core.Constant;
using NoughtsAndCrosses.Core.Domain;
using NoughtsAndCrosses.Core.Domain.Exceptions;
using NoughtsAndCrosses.Core.Enum;

namespace NoughtsAndCrosses.ConsoleApp.Tests;

public class GameSpecifications : IDisposable
{
    public void Dispose()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.ResetGame();
        
        AppManager appManager = AppManager.Instance;
        appManager.ChangeScreen(AppScreen.Menu);
    }
    
    [Fact]
    public void Should_not_have_a_game_initially()
    {
        GameManager gameManager = GameManager.Instance;

        Action act = () => { var foundGame= gameManager.Game; };

        act.Should().Throw<NullReferenceException>();
    }
    
    [Fact]
    public void Should_throw_exception_if_game_starts_with_less_than_two_players()
    {
        AppManager appManager = AppManager.Instance;
        appManager.ChangeScreen(AppScreen.InGame);
        GameManager gameManager = GameManager.Instance;
        
        Action act = () => gameManager.NewGame();
        
        act.Should().Throw<Exception>().WithMessage("There must be at least 2 players to start a game.");
    }
    
    [Fact]
    public void Should_start_game_if_game_has_two_players()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.AddPlayer(new Player(Mark.X));
        gameManager.AddPlayer(new Player(Mark.O));
        gameManager.ClientPlayer = gameManager.Players.First();
        gameManager.NewGame();
        
        gameManager.Game.Should().NotBeNull();
    }
    
    [Fact]
    public void Should_assign_X_player_with_first_turn()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.AddPlayer(new Player(Mark.X));
        gameManager.AddPlayer(new Player(Mark.O));
        gameManager.ClientPlayer = gameManager.Players.First();
        
        gameManager.NewGame();
        
        gameManager.TurnPlayer.AssignedMark.Should().Be(Mark.X);
    }
    
    [Fact]
    public void Should_switch_turns_after_every_move()
    {
        AppManager appManager = AppManager.Instance;
        appManager.ChangeScreen(AppScreen.InGame);
        GameManager gameManager = GameManager.Instance;
        var playerX = gameManager.AddPlayer(new Player(Mark.X));
        var playerO = gameManager.AddPlayer(new Player(Mark.O));
        gameManager.ClientPlayer = playerX;
        gameManager.NewGame();
        playerX.PlaceMark(new Coordinate(FileLetter.A, 1));
        
        gameManager.TurnPlayer.AssignedMark.Should().Be(Mark.O);
        
        playerO.PlaceMark(new Coordinate(FileLetter.A, 2));
        gameManager.TurnPlayer.AssignedMark.Should().Be(Mark.X);
    }

    [Fact]
    public void Should_throw_exception_if_player_moves_when_game_has_ended()
    {
        AppManager appManager = AppManager.Instance;
        appManager.ChangeScreen(AppScreen.InGame);
        GameManager gameManager = GameManager.Instance;
        var playerX = gameManager.AddPlayer(new Player(Mark.X));
        var playerO = gameManager.AddPlayer(new Player(Mark.O));
        gameManager.ClientPlayer = playerX;
        gameManager.NewGame();
        
        playerX.PlaceMark(new Coordinate(FileLetter.A, 1));
        playerO.PlaceMark(new Coordinate(FileLetter.B, 1));
        playerX.PlaceMark(new Coordinate(FileLetter.A, 2));
        playerO.PlaceMark(new Coordinate(FileLetter.B, 2));
        playerX.PlaceMark(new Coordinate(FileLetter.A, 3)); // winning move
        Action act = () => playerO.PlaceMark(new Coordinate(FileLetter.C, 1));

        act.Should().Throw<Exception>();
    }
    
    
    [Theory]
    [InlineData( new string[] {"a1", "a2", "a3"}, new string[] {"b1", "b2", "c3"} )]
    [InlineData( new string[] {"a1", "B2", "c3"}, new string[] {"b1", "b3", "c1"} )]
    public void Should_win_when_three_marks_are_in_a_row(string[] xCoordinates, string[] oCoordinates)
    {
        // Arrange
        var appManager = AppManager.Instance;
        appManager.ChangeScreen(AppScreen.InGame);
        var gameManager = GameManager.Instance;
        var playerX = gameManager.AddPlayer(new Player(Mark.X));
        var playerO = gameManager.AddPlayer(new Player(Mark.O));
        gameManager.ClientPlayer = playerX;
        gameManager.NewGame();
        
        // Act
        for (int i = 0; i < xCoordinates.Length; i++)
        {
            var xCoordinate = xCoordinates[i];
            var oCoordinate = oCoordinates[i];
            playerX.PlaceMark(Coordinate.Parse(xCoordinate));
            if (i == xCoordinates.Length - 1) break; // ignore the last iteration for "O" since X has won.
            playerO.PlaceMark(Coordinate.Parse(oCoordinate));
        }
        
        // Assert
        gameManager.Game.Winner.Should().NotBe(null);
    }

    [Fact]
    public void Should_reset_game_when_restarting()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.AddPlayer(new Player(Mark.X));
        gameManager.AddPlayer(new Player(Mark.O));
        gameManager.ClientPlayer = gameManager.Players.First();
        gameManager.NewGame();

        gameManager.ResetGame();
        Action getGameAction = () =>
        {
            var getGame= gameManager.Game;
            var getClientPlayer = gameManager.ClientPlayer;
        };

        gameManager.Players.Should().BeEmpty(); // Should probably be moved to Game class.
        gameManager.TurnPlayer.Should().BeNull(); // Should probably be moved to Game class.
        getGameAction.Should().Throw<NullReferenceException>();
    }
    
    [Theory]
    [InlineData(Mark.X, "a1")]
    [InlineData(Mark.X, "A2")]
    public void Should_place_mark_if_valid_coordinate_input(Mark markType, string input)
    {
        var appManager = AppManager.Instance;
        appManager.ChangeScreen(AppScreen.InGame);
        var gameManager = GameManager.Instance;
        var playerX = gameManager.AddPlayer(new Player(Mark.X));
        var playerO = gameManager.AddPlayer(new Player(Mark.O));
        gameManager.ClientPlayer = playerX;
        gameManager.NewGame();
        
        appManager.HandleInput(input); // triggers the method that places the mark on the board
        Space affectedSpace = appManager.GameManager.Game.GetSpace(input); 
        
        affectedSpace.Mark.Should().Be(appManager.GameManager.ClientPlayer.AssignedMark);
    }

    [Fact]
    public void Should_throw_exception_if_player_moves_when_not_their_turn()
    {
        throw new NotImplementedException();
    }


    [Theory]
    [InlineData(Mark.X, "aa")]
    [InlineData(Mark.X, "HelloWorld")]
    [InlineData(Mark.X, "11")]
    public void Should_throw_exception_if_invalid_coordinate_input(Mark markType, string input)
    {
        var appManager = AppManager.Instance;
        appManager.ChangeScreen(AppScreen.InGame);
        var gameManager = GameManager.Instance;
        var playerX = gameManager.AddPlayer(new Player(Mark.X));
        var playerO = gameManager.AddPlayer(new Player(Mark.O));
        gameManager.ClientPlayer = playerX;
        gameManager.NewGame();
        
        Action act = () => appManager.HandleInput(input);
        
        act.Should().Throw<Exception>();
    }
    
 
    [Fact]
    public void Should_exit_game_with_exit_input()
    {
        var gameManager = AppManager.Instance;
        gameManager.ChangeScreen(AppScreen.Menu); // TODO: should take any screen
        
        gameManager.HandleInput(GeneralCommand.CloseApplication);
        
        gameManager.IsListeningForInputs.Should().BeFalse();
    }
    
    [Theory]
    [InlineData(AppScreen.InGame)]
    [InlineData(AppScreen.HostGame)]
    public void Should_navigate_to_menu_with_back_input(AppScreen appScreen)
    {
        var gameManager = AppManager.Instance;
        gameManager.ChangeScreen(appScreen);
        
        gameManager.HandleInput(GeneralCommand.Back);
        
        gameManager.CurrentScreen.Should().Be(AppScreen.Menu);
    }
    
    [Fact]
    public void Should_log_invalid_command_if_given()
    {
        var gameManager = AppManager.Instance;
        gameManager.ChangeScreen(AppScreen.Menu);
        
        bool worked = gameManager.Screens[gameManager.CurrentScreen].HandleInput("ASDF");
        
        throw new NotImplementedException();
        // worked.Should().Be(false);
        // _consoleService.SystemMessage(GameScreen.Menu, "Invalid command.").Should().BeTrue();
    }
    
    
    [Fact]
    public void Should_display_a_3x3_board()
    {
        var gameManager = GameManager.Instance;
        var playerX = gameManager.AddPlayer(new Player(Mark.X));
        var playerO = gameManager.AddPlayer(new Player(Mark.O));
        gameManager.ClientPlayer = playerX;
        gameManager.NewGame();
        
        using (var consoleOutput = new StringWriter()) // We need to capture the output of the console to assert
        {
            Console.SetOut(consoleOutput);
            
            // Act
            gameManager.Game.ShowBoard();
            
            // Assert
            var expectedOutput = "3 [ ][ ][ ]\n2 [ ][ ][ ]\n1 [ ][ ][ ]\n";
            consoleOutput.ToString().Should().Contain(expectedOutput);
        }

        // Reset the console output
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
    }
    
    [Fact]
    public void Should_throw_exception_if_player_marks_an_occupied_space()
    {
        // Arrange      
        var appManager = AppManager.Instance;
        appManager.ChangeScreen(AppScreen.InGame);
        var gameManager = GameManager.Instance;
        var xPlayer = gameManager.AddPlayer(new Player(Mark.X));
        var oPlayer = gameManager.AddPlayer(new Player(Mark.O));
        gameManager.ClientPlayer = xPlayer;
        gameManager.NewGame();
        
        // Act
        Action act = () =>
        {
            xPlayer.PlaceMark(new Coordinate(FileLetter.A, 1));
            oPlayer.PlaceMark(new Coordinate(FileLetter.A, 1));
        };

        // Assert
        act.Should().Throw<SpaceOccupiedException>();
    }
    
    
    [Fact]
    public void Should_draw_when_all_spaces_are_occupied_and_no_winner()
    {
        // Arrange      
        var appManager = AppManager.Instance;
        appManager.ChangeScreen(AppScreen.InGame);
        var gameManager = GameManager.Instance;
        var xPlayer = gameManager.AddPlayer(new Player(Mark.X));
        var oPlayer = gameManager.AddPlayer(new Player(Mark.O));
        gameManager.ClientPlayer = xPlayer;
        gameManager.NewGame();
        
        // Act
        xPlayer.PlaceMark(new Coordinate(FileLetter.A, 1));
        oPlayer.PlaceMark(new Coordinate(FileLetter.A, 3));
        xPlayer.PlaceMark(new Coordinate(FileLetter.A, 2));
        oPlayer.PlaceMark(new Coordinate(FileLetter.B, 1));
        xPlayer.PlaceMark(new Coordinate(FileLetter.B, 3));
        oPlayer.PlaceMark(new Coordinate(FileLetter.B, 2));
        xPlayer.PlaceMark(new Coordinate(FileLetter.C, 3));
        oPlayer.PlaceMark(new Coordinate(FileLetter.C, 2));
        xPlayer.PlaceMark(new Coordinate(FileLetter.C, 1));
        
        // Assert
        gameManager.Game.GetGameResult().Should().Be(GameResult.Draw);
    }
    
    [Fact]
    public void Should_win_when_all_spaces_are_occupied_and_last_move_is_winning_move()
    {
        // Arrange      
        var appManager = AppManager.Instance;
        appManager.ChangeScreen(AppScreen.InGame);
        var gameManager = GameManager.Instance;
        var xPlayer = gameManager.AddPlayer(new Player(Mark.X));
        var oPlayer = gameManager.AddPlayer(new Player(Mark.O));
        gameManager.ClientPlayer = xPlayer;
        gameManager.NewGame();
        
        // Act
        xPlayer.PlaceMark(new Coordinate(FileLetter.A, 1));
        oPlayer.PlaceMark(new Coordinate(FileLetter.A, 3));
        xPlayer.PlaceMark(new Coordinate(FileLetter.A, 2));
        oPlayer.PlaceMark(new Coordinate(FileLetter.B, 2));
        xPlayer.PlaceMark(new Coordinate(FileLetter.B, 1));
        oPlayer.PlaceMark(new Coordinate(FileLetter.C, 3));
        xPlayer.PlaceMark(new Coordinate(FileLetter.B, 3));
        oPlayer.PlaceMark(new Coordinate(FileLetter.C, 2));
        xPlayer.PlaceMark(new Coordinate(FileLetter.C, 1)); // Fills the board and winning move.
        
        // Assert
        gameManager.Game.GetGameResult().Should().Be(GameResult.SomeoneWon);
        gameManager.Game.Winner.Should().Be(xPlayer);
    }
}