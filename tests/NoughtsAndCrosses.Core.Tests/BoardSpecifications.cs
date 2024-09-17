using FluentAssertions;
using NoughtsAndCrosses.Core.Domain;
using NoughtsAndCrosses.Core.Enum;
using System;
using NoughtsAndCrosses.ConsoleApp;
using NoughtsAndCrosses.ConsoleApp.Domain;

namespace NoughtsAndCrosses.Core;

public class BoardSpecifications
{
    [Fact]
    public void Should_have_9_spaces()
    {
        // Arrange
        var gameManager = new GameManager();
        
        // Act
        
        
        // Assert
        gameManager.Game.Board.Spaces.Length.Should().Be(9);
    }
    
    [Fact]
    public void Should_display_a_3x3_board()
    {
        // Arrange
        var gameManager = new GameManager();
        
        using (var consoleOutput = new StringWriter()) // We need to capture the output of the console to assert
        {
            Console.SetOut(consoleOutput);
            
            // Act
            gameManager.Game.Board.ShowBoard();

            // Assert
            var expectedOutput = "3 [ ][ ][ ]\n2 [ ][ ][ ]\n1 [ ][ ][ ]\n";
            // var expectedOutput = "[a3][b3][c3]\n[a2][b2][c2]\n[a1][b1][c1]\n";
            consoleOutput.ToString().Should().Contain(expectedOutput);
        }

        // Reset the console output
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
    }
    
    [Theory]
    [InlineData(FileLetter.A, RankNumber.One, Mark.X)]
    [InlineData(FileLetter.C, RankNumber.Three, Mark.O)]
    [InlineData(FileLetter.B, RankNumber.Two, Mark.O)]
    public void Should_be_able_to_place_mark_on_space(FileLetter fileLetter, RankNumber rankNumber, Mark mark)
    {
        // Arrange
        var gameManager = new GameManager();
        
        // Act
        gameManager.Game.Board.PlaceMark(new Coordinate(fileLetter, rankNumber), mark);

        // Assert
        gameManager.Game.Board.Spaces.First(s => s.Coordinate.Value == $"{fileLetter}{(int)rankNumber}").Mark.Should().Be(mark);
    }
    
    [Fact]
    public void Should_throw_exception_when_placing_mark_on_occupied_space()
    {
        // Arrange
        var gameManager = new GameManager();
        
        // Act
        gameManager.Game.Board.PlaceMark(new Coordinate(FileLetter.A, RankNumber.One), Mark.X);
        
        // Assert
        Action act = () => gameManager.Game.Board.PlaceMark(new Coordinate(FileLetter.A, RankNumber.One), Mark.O);
        act.Should().Throw<Exception>().WithMessage("Space is already occupied");
    }
}
