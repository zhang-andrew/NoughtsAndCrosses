using FluentAssertions;

namespace NoughtsAndCrosses.Core.Tests;

public class BoardSpecifications
{
    [Fact]
    public void Should_have_9_spaces()
    {
        // Arrange
        var gameManager = new GameManager();
        
        // Act
        Game game = gameManager.NewGame();
        
        // Assert
        game.Board.Spaces.Length.Should().Be(9);
    }
    
    [Fact]
    public void Should_display_a_3x3_board()
    {
        // Arrange
        var gameManager = new GameManager();
        Game newGame = gameManager.NewGame();
        
        using (var consoleOutput = new StringWriter()) // We need to capture the output of the console to assert
        {
            Console.SetOut(consoleOutput);
            
            // Act
            newGame.Board.ShowBoard();

            // Assert
            var expectedOutput = "[ ][ ][ ]\n[ ][ ][ ]\n[ ][ ][ ]\n";
            // var expectedOutput = "[a3][b3][c3]\n[a2][b2][c2]\n[a1][b1][c1]\n";
            consoleOutput.ToString().Should().Be(expectedOutput);
        }
    }
}

public class GameManager
{
    public Game NewGame()
    {
        return new Game();
    }
}

public class Game
{
    public Board Board { get; }
    
    public Game()
    {
        Board = new Board();       
    }
    
    
}

public class Board
{
    public Space[] Spaces { get; } = new Space[9]
    {
        new Space("a3"),
        new Space("b3"),
        new Space("c3"),
        new Space("a2"),
        new Space("b2"),
        new Space("c2"),
        new Space("a1"),
        new Space("b1"),
        new Space("c1")
    };

    public void ShowBoard()
    {
        for (int i = 0; i < Spaces.Length; i++)
        {
            string mark = Spaces[i].Mark == Mark.Empty ? " " : Spaces[i].Mark.ToString();
            
            if ((i+1) % 3 == 0)
            {
                Console.WriteLine($"[{mark}]");
            } else {
                Console.Write($"[{mark}]");
            }    
        }
    }
}

public class Space
{
    public Mark Mark { get; set; } = Mark.Empty;
    public string Coordinate { get; }

    public Space(string coordinate)
    {
        Coordinate = coordinate;
    }
}

public enum Mark
{
    Empty,
    X,
    O
}