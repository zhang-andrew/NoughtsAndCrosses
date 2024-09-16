using FluentAssertions;

namespace NoughtsAndCrosses.Core.Tests;

public class BoardSpecifications
{
    [Fact]
    public void Should_have_a_board_with_9_cells()
    {
        // Arrange
        var gameManager = new GameManager();
        
        // Act
        Game game = gameManager.NewGame();
        
        // Assert
        game.Board.Length.Should().Be(9);
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
            newGame.ShowBoard();

            // Assert
            var expectedOutput = "[ ][ ][ ]\n[ ][ ][ ]\n[ ][ ][ ]\n";
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
    public string[] Board { get; } = new string[9];
    
    public void ShowBoard()
    {
        for (int i = 0; i < Board.Length; i++)
        {
            if ((i+1) % 3 == 0)
            {
                Console.WriteLine("[ ]");
            } else {
                Console.Write("[ ]");
            }    
        }
    }
}