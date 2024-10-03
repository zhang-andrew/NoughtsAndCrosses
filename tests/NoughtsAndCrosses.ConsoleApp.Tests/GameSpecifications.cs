using FluentAssertions;
using NoughtsAndCrosses.Core.Constant;
using NoughtsAndCrosses.Core.Domain;
using NoughtsAndCrosses.Core.Domain.Exceptions;
using NoughtsAndCrosses.Core.Enum;

namespace NoughtsAndCrosses.ConsoleApp.Tests;

public class GameSpecifications : IDisposable
{
    private readonly Player _playerX;
    private readonly Player _playerO;
    
    public GameSpecifications() // runs before each test
    {
        // Prepare the game to be InGame screen
        AppManager appManager = AppManager.Instance;
        appManager.ChangeScreen(AppScreen.InGame);
        
        // Create two players
        _playerX = new Player(Mark.X);
        _playerO = new Player(Mark.O);
    }
    
    public void Dispose()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.ResetGame();
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
        GameManager gameManager = GameManager.Instance;
        
        Action act = () => gameManager.NewLocalGame(_playerX, null);
        
        act.Should().Throw<Exception>().WithMessage("There must be at least 2 players to start a game.");
    }
    
    [Fact]
    public void Should_start_game_if_game_has_two_players()
    {
        GameManager gameManager = GameManager.Instance;
        // gameManager.Game.AddPlayer(_playerX);
        // gameManager.Game.AddPlayer(_playerO);
        gameManager.ClientPlayer = _playerX;
        gameManager.NewLocalGame(_playerX, _playerO);
        
        gameManager.Game.Should().NotBeNull();
    }
    
    [Fact]
    public void Should_assign_X_player_with_first_turn()
    {
        GameManager gameManager = GameManager.Instance;
        // gameManager.Game.AddPlayer(_playerX);
        // gameManager.Game.AddPlayer(_playerO);
        gameManager.ClientPlayer = _playerX;
        
        gameManager.NewLocalGame(_playerX, _playerO);
        
        gameManager.Game.TurnPlayer.AssignedMark.Should().Be(Mark.X);
    }
    
    [Fact]
    public void Should_switch_turns_after_every_move()
    {
        GameManager gameManager = GameManager.Instance;
        // gameManager.Game.AddPlayer(_playerX);
        // gameManager.Game.AddPlayer(_playerO);
        gameManager.ClientPlayer = _playerX;
        gameManager.NewLocalGame(_playerX, _playerO);
        
        _playerX.PlaceMark(new Coordinate(FileLetter.A, 1));
        gameManager.Game.TurnPlayer.AssignedMark.Should().Be(Mark.O);
        
        _playerO.PlaceMark(new Coordinate(FileLetter.A, 2));
        gameManager.Game.TurnPlayer.AssignedMark.Should().Be(Mark.X);
    }

    [Fact]
    public void Should_throw_exception_if_player_moves_when_game_has_ended()
    {
        var gameManager = GameManager.Instance;
        
        gameManager.ClientPlayer = _playerX;
        gameManager.NewLocalGame(_playerX, _playerO);
        
        _playerX.PlaceMark(new Coordinate(FileLetter.A, 1));
        _playerO.PlaceMark(new Coordinate(FileLetter.B, 1));
        _playerX.PlaceMark(new Coordinate(FileLetter.A, 2));
        _playerO.PlaceMark(new Coordinate(FileLetter.B, 2));
        _playerX.PlaceMark(new Coordinate(FileLetter.A, 3)); // winning move
        Action act = () => _playerO.PlaceMark(new Coordinate(FileLetter.C, 1));

        act.Should().Throw<Exception>();
    }
    
    
    [Theory]
    [InlineData( new string[] {"a1", "a2", "a3"}, new string[] {"b1", "b2", "c3"} )]
    [InlineData( new string[] {"a1", "B2", "c3"}, new string[] {"b1", "b3", "c1"} )]
    public void Should_win_when_three_marks_are_in_a_row(string[] xCoordinates, string[] oCoordinates)
    {
        // Arrange
        var gameManager = GameManager.Instance;
        
        gameManager.ClientPlayer = _playerX;
        gameManager.NewLocalGame(_playerX, _playerO);
        
        // Act
        for (int i = 0; i < xCoordinates.Length; i++)
        {
            var xCoordinate = xCoordinates[i];
            var oCoordinate = oCoordinates[i];
            _playerX.PlaceMark(Coordinate.Parse(xCoordinate));
            if (i == xCoordinates.Length - 1) break; // ignore the last iteration for "O" since X has won.
            _playerO.PlaceMark(Coordinate.Parse(oCoordinate));
        }
        
        // Assert
        gameManager.Game.Winner.Should().NotBe(null);
    }

    [Fact]
    public void Should_reset_game_when_restarting()
    {
        GameManager gameManager = GameManager.Instance;
        
        gameManager.ClientPlayer = _playerX;
        gameManager.NewLocalGame(_playerX, _playerO);

        gameManager.ResetGame();
        Action getGameAction = () =>
        {
            var getGame= gameManager.Game;
            var getClientPlayer = gameManager.ClientPlayer;
        };

        getGameAction.Should().Throw<NullReferenceException>();
    }
    
    [Theory]
    [InlineData(Mark.X, "a1")]
    [InlineData(Mark.X, "A2")]
    [InlineData(Mark.X, "1a")]
    [InlineData(Mark.X, "1A")]
    public void Should_place_mark_if_valid_coordinate_input(Mark markType, string input)
    {
        var appManager = AppManager.Instance;
        appManager.ChangeScreen(AppScreen.InGame);
        var gameManager = GameManager.Instance;
        
        gameManager.ClientPlayer = _playerX;
        gameManager.NewLocalGame(_playerX, _playerO);
        
        appManager.WriteInput(input); // triggers the method that places the mark on the board
        Space affectedSpace = appManager.GameManager.Game.GetSpace(input); 
        
        affectedSpace.Mark.Should().Be(appManager.GameManager.ClientPlayer.AssignedMark);
    }

    [Fact]
    public void Should_throw_exception_if_player_moves_when_not_their_turn()
    {
        GameManager gameManager = GameManager.Instance;
        
        gameManager.ClientPlayer = _playerX;
        gameManager.NewLocalGame(_playerX, _playerO);


        Action act = () =>
        {
            _playerX.PlaceMark(new Coordinate(FileLetter.A, 1)); // X goes first
            _playerO.PlaceMark(new Coordinate(FileLetter.B, 1)); // 0 next move
            _playerO.PlaceMark(new Coordinate(FileLetter.B, 2)); // 0 tries to move again
        };

        // act.Should().Throw<Exception>();
        act.Should().Throw<Exception>().WithMessage("It's not your turn.");
    }


    [Theory]
    [InlineData(Mark.X, "aa")]
    [InlineData(Mark.X, "%%")]
    [InlineData(Mark.X, "11")]
    public void Should_log_invalid_coordinate_input(Mark markType, string input)
    {
        var appManager = AppManager.Instance;
        appManager.ChangeScreen(AppScreen.InGame);
        var gameManager = GameManager.Instance;
        
        gameManager.ClientPlayer = _playerX;
        gameManager.NewLocalGame(_playerX, _playerO);
        
        var consoleOutput = new StringWriter(); // Capture Console.WriteLine output
        Console.SetOut(consoleOutput);
        
        // Act
        appManager.WriteInput(input);
        
        // Assert
        consoleOutput.ToString().Should().Contain("Invalid input");
        // act.Should().Throw<InvalidCoordinateException>();
        
        // Cleanup - Reset the console output
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
    }
    
    [Theory]
    [InlineData(AppScreen.Menu)]
    [InlineData(AppScreen.HostGame)]
    [InlineData(AppScreen.JoinGame)]
    [InlineData(AppScreen.PreGame)]
    [InlineData(AppScreen.InGame)]
    [InlineData(AppScreen.PostGame)]
    public void Should_log_invalid_inputs(AppScreen screen)
    {
        // Arrange
        if (screen == AppScreen.InGame || screen == AppScreen.PostGame)
        {
            var gameManager = GameManager.Instance;
            gameManager.ClientPlayer = _playerX;
            gameManager.NewLocalGame(_playerX, _playerO);
        }
        
        var appManager = AppManager.Instance;
        appManager.ChangeScreen(screen);
        
        var consoleOutput = new StringWriter(); // Capture Console.WriteLine output
        Console.SetOut(consoleOutput);
    
        // Act
        appManager.WriteInput("ASDF");
        appManager.WriteInput("AA");
        appManager.WriteInput("11");
        
        // Assert
        consoleOutput.ToString().Should().Contain("Invalid input");
        
        
        // Cleanup - Reset the console output
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
    }
 
    [Fact]
    public void Should_exit_game_with_exit_input()
    {
        var gameManager = AppManager.Instance;
        gameManager.ChangeScreen(AppScreen.Menu); // TODO: should take any screen
        
        gameManager.WriteInput(GeneralCommand.CloseApplication);
        
        gameManager.IsListeningForInputs.Should().BeFalse();
    }
    
    [Theory]
    [InlineData(AppScreen.JoinGame)]
    [InlineData(AppScreen.PreGame)]
    [InlineData(AppScreen.HostGame)]
    public void Should_navigate_to_menu_with_back_input(AppScreen appScreen)
    {
        var gameManager = AppManager.Instance;
        gameManager.ChangeScreen(appScreen);
        
        gameManager.WriteInput(GeneralCommand.Back);
        
        gameManager.CurrentScreen.Should().Be(AppScreen.Menu);
    }
    
    [Theory]
    [InlineData(AppScreen.InGame)]
    public void Should_not_navigate_to_menu_with_back_input(AppScreen appScreen)
    {
        var gameManager = AppManager.Instance;
        gameManager.ChangeScreen(appScreen);
        
        gameManager.WriteInput(GeneralCommand.Back);
        
        gameManager.CurrentScreen.Should().Be(appScreen);
    }
    
    
    
    [Fact]
    public void Should_display_a_3x3_board()
    {
        var gameManager = GameManager.Instance;
        
        gameManager.ClientPlayer = _playerX;
        gameManager.NewLocalGame(_playerX, _playerO);
        
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
    public void Should_log_invalid_player_mark_on_occupied_space()
    {
        // Arrange      
        var appManager = AppManager.Instance;
        appManager.ChangeScreen(AppScreen.InGame);
        var gameManager = GameManager.Instance;
        gameManager.ClientPlayer = _playerO;
        gameManager.NewLocalGame(_playerX, _playerO);
        
        var consoleOutput = new StringWriter(); // Capture Console.WriteLine output
        Console.SetOut(consoleOutput);
        
        // Act
        _playerX.PlaceMark(new Coordinate(FileLetter.A, 1));
        // oPlayer.PlaceMark(new Coordinate(FileLetter.A, 1)); // this action will throw the exception, because we directly called the method
        appManager.WriteInput("a1"); // this will catch the exception, handle it by logging "Invalid input".
        
        // Assert
        consoleOutput.ToString().Should().Contain("Invalid input");
        
        // Cleanup - Reset the console output
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
    }
    
    [Fact]
    public void Should_draw_when_all_spaces_are_occupied_and_no_winner()
    {
        // Arrange      
        var appManager = AppManager.Instance;
        appManager.ChangeScreen(AppScreen.InGame);
        var gameManager = GameManager.Instance;
        // var xPlayer = gameManager.Game.AddPlayer(_playerX);
        // var oPlayer = gameManager.Game.AddPlayer(_playerO);
        gameManager.ClientPlayer = _playerX;
        gameManager.NewLocalGame(_playerX, _playerO);
        
        // Act
        _playerX.PlaceMark(new Coordinate(FileLetter.A, 1));
        _playerO.PlaceMark(new Coordinate(FileLetter.A, 3));
        _playerX.PlaceMark(new Coordinate(FileLetter.A, 2));
        _playerO.PlaceMark(new Coordinate(FileLetter.B, 1));
        _playerX.PlaceMark(new Coordinate(FileLetter.B, 3));
        _playerO.PlaceMark(new Coordinate(FileLetter.B, 2));
        _playerX.PlaceMark(new Coordinate(FileLetter.C, 3));
        _playerO.PlaceMark(new Coordinate(FileLetter.C, 2));
        _playerX.PlaceMark(new Coordinate(FileLetter.C, 1));
        
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
        gameManager.ClientPlayer = _playerX;
        gameManager.NewLocalGame(_playerX, _playerO);
        
        // Act
        _playerX.PlaceMark(new Coordinate(FileLetter.A, 1));
        _playerO.PlaceMark(new Coordinate(FileLetter.A, 3));
        _playerX.PlaceMark(new Coordinate(FileLetter.A, 2));
        _playerO.PlaceMark(new Coordinate(FileLetter.B, 2));
        _playerX.PlaceMark(new Coordinate(FileLetter.B, 1));
        _playerO.PlaceMark(new Coordinate(FileLetter.C, 3));
        _playerX.PlaceMark(new Coordinate(FileLetter.B, 3));
        _playerO.PlaceMark(new Coordinate(FileLetter.C, 2));
        _playerX.PlaceMark(new Coordinate(FileLetter.C, 1)); // Fills the board and winning move.
        
        // Assert
        gameManager.Game.GetGameResult().Should().Be(GameResult.SomeoneWon);
        gameManager.Game.Winner.Should().Be(_playerX);
    }
}