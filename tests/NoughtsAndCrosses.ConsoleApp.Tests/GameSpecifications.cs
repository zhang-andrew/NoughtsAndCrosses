using FluentAssertions;
using NoughtsAndCrosses.Core.Constant;
using NoughtsAndCrosses.Core.Domain;
using NoughtsAndCrosses.Core.Enum;

namespace NoughtsAndCrosses.ConsoleApp.Tests;

public class GameSpecifications : IDisposable
{
    public void Dispose()
    {
        AppManager appManager = AppManager.Instance;
        appManager.ChangeScreen(AppScreen.Menu);
        
        GameManager gameManager = GameManager.Instance;
        gameManager.ResetGame();
    }
    
    [Fact]
    public void Should_not_have_a_game_initially()
    {
        GameManager gameManager = GameManager.Instance;
        
        // 
        
        gameManager.Game.Should().BeNull();
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
        
        gameManager.NewGame();
        
        gameManager.Game.Should().NotBeNull();
    }
    
    [Fact]
    public void Should_assign_X_player_to_have_first_turn()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.AddPlayer(new Player(Mark.X));
        gameManager.AddPlayer(new Player(Mark.O));
        
        gameManager.NewGame();
        
        gameManager.TurnPlayer.AssignedMark.Should().Be(Mark.X);
    }
    
    [Fact]
    public void Should_throw_exception_if_X_player_does_not_have_first_turn()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.AddPlayer(new Player(Mark.X));
        gameManager.AddPlayer(new Player(Mark.O));
        
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
        gameManager.NewGame();
        
        playerX.PlaceMark(new Coordinate(FileLetter.A, 1));
        playerO.PlaceMark(new Coordinate(FileLetter.B, 1));
        playerX.PlaceMark(new Coordinate(FileLetter.A, 2));
        playerO.PlaceMark(new Coordinate(FileLetter.B, 2));
        playerX.PlaceMark(new Coordinate(FileLetter.A, 3)); // winning move
        Action act = () => playerO.PlaceMark(new Coordinate(FileLetter.C, 1));

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void Should_reset_game_when_restarting()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.AddPlayer(new Player(Mark.X));
        gameManager.AddPlayer(new Player(Mark.O));
        gameManager.NewGame();

        gameManager.ResetGame();
        
        gameManager.Game.Should().BeNull();
    }
}