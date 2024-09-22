using FluentAssertions;
using NoughtsAndCrosses.Core.Domain;
using NoughtsAndCrosses.Core.Enum;
using System;
using System.Runtime.CompilerServices;
// using NoughtsAndCrosses.ConsoleApp;
// using NoughtsAndCrosses.ConsoleApp.Domain;
using NoughtsAndCrosses.Core.Constant;
using NoughtsAndCrosses.Core.Domain.Exceptions;

namespace NoughtsAndCrosses.Core;

public class BoardSpecifications
{
    [Fact]
    public void Should_have_9_spaces()
    {
        // Arrange      
        var gameManager = GameManager.Instance;
        
        // Act
        
        
        // Assert
        gameManager.Game.Spaces.Length.Should().Be(9);
    }
    
    [Fact]
    public void Should_display_a_3x3_board()
    {
        // Arrange      
        var gameManager = GameManager.Instance;
        
        using (var consoleOutput = new StringWriter()) // We need to capture the output of the console to assert
        {
            Console.SetOut(consoleOutput);
            
            // Act
            gameManager.Game.ShowBoard();

            // Assert
            var expectedOutput = "3 [ ][ ][ ]\n2 [ ][ ][ ]\n1 [ ][ ][ ]\n";
            // var expectedOutput = "[a3][b3][c3]\n[a2][b2][c2]\n[a1][b1][c1]\n";
            consoleOutput.ToString().Should().Contain(expectedOutput);
        }

        // Reset the console output
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
    }
    
    [Theory]
    [InlineData(FileLetter.A, 1, Mark.X)]
    [InlineData(FileLetter.C, 3, Mark.O)]
    [InlineData(FileLetter.B, 2, Mark.O)]
    public void Should_be_able_to_place_mark_on_space(string fileLetter, int rankNumber, Mark mark)
    {
        // Arrange      
        var gameManager = GameManager.Instance;
        
        // Act
        gameManager.Game.PlaceMark(new Coordinate(fileLetter, rankNumber), mark);

        // Assert
        gameManager.Game.Spaces.First(s => s.Coordinate.Value == $"{fileLetter}{(int)rankNumber}").Mark.Should().Be(mark);
    }
    
    [Fact]
    public void Should_throw_exception_when_placing_mark_on_occupied_space()
    {
        // Arrange      
        var gameManager = GameManager.Instance;
        
        // Act
        gameManager.Game.PlaceMark(new Coordinate(FileLetter.A, 1), Mark.X);
        Action act = () => gameManager.Game.PlaceMark(new Coordinate(FileLetter.A, 1), Mark.O);
        
        // Assert
        act.Should().Throw<SpaceOccupiedException>();
    }
    
    [Theory]
    [InlineData(new object[] { new string[] {"a1", "a2", "a3"}})]
    [InlineData(new object[] { new string[] {"a1", "B2", "c3"}})]
    public void Should_win_when_three_marks_are_in_a_row(string[] coordinates)
    {
        // Arrange      
        var gameManager = GameManager.Instance;
        
        // Act
        foreach (string coordinate in coordinates)
        {
            gameManager.Game.PlaceMark(Coordinate.Parse(coordinate), Mark.X);
        }
        
        // Assert
        gameManager.Game.Winner.Should().NotBe(null);
    }
    
    [Fact]
    public void Should_draw_when_all_spaces_are_occupied_and_no_winner()
    {
        // Arrange      
        var gameManager = GameManager.Instance;
        
        // Act
        gameManager.Game.PlaceMark(new Coordinate(FileLetter.A, 1), Mark.X);
        gameManager.Game.PlaceMark(new Coordinate(FileLetter.A, 2), Mark.X);
        gameManager.Game.PlaceMark(new Coordinate(FileLetter.A, 3), Mark.O);
        gameManager.Game.PlaceMark(new Coordinate(FileLetter.B, 1), Mark.O);
        gameManager.Game.PlaceMark(new Coordinate(FileLetter.B, 2), Mark.O);
        gameManager.Game.PlaceMark(new Coordinate(FileLetter.B, 3), Mark.X);
        gameManager.Game.PlaceMark(new Coordinate(FileLetter.C, 1), Mark.X);
        gameManager.Game.PlaceMark(new Coordinate(FileLetter.C, 2), Mark.O);
        gameManager.Game.PlaceMark(new Coordinate(FileLetter.C, 3), Mark.X);
        
        // Assert
        gameManager.Game.GameResult.Should().Be(GameResult.Draw);
    }
    
    // [Fact]
    // public void Should_indicate_that_X_always_start_first()
    // {
    //     // Arrange      
    //     var gameManager = GameManager.Instance;
    //     
    //     // Act
    //     gameManager.Board.StartGame();
    //     gameManager.LocalPlayer.AssignedMark = Mark.O;
    //     gameManager.Board.PlaceMark(new Coordinate(FileLetter.A, 1), Mark.O);
    //     
    //     // Assert
    //     gameManager.Board.Spaces.Any(s => s.Coordinate.Value == "a1").Mark.Should().Be(Mark.X);
    // }
}
