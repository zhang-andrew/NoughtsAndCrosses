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
        Game newGame = gameManager.NewGame();
        
        // Act
        newGame.Board.PlaceMark(new Coordinate(fileLetter, rankNumber), mark);

        // Assert
        newGame.Board.Spaces.First(s => s.Coordinate.Value == $"{fileLetter}{(int)rankNumber}").Mark.Should().Be(mark);
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
        new Space(new Coordinate(FileLetter.A, RankNumber.Three)),
        new Space(new Coordinate(FileLetter.B, RankNumber.Three)),
        new Space(new Coordinate(FileLetter.C, RankNumber.Three)),
        new Space(new Coordinate(FileLetter.A, RankNumber.Two)),
        new Space(new Coordinate(FileLetter.B, RankNumber.Two)),
        new Space(new Coordinate(FileLetter.C, RankNumber.Two)),
        new Space(new Coordinate(FileLetter.A, RankNumber.One)),
        new Space(new Coordinate(FileLetter.B, RankNumber.One)),
        new Space(new Coordinate(FileLetter.C, RankNumber.One))
    };

    public void ShowBoard()
    {
        for (int i = 0; i < Spaces.Length; i++)
        {
            
            if (i % 3 == 0)
            {
                string rankAsString = ((int)Spaces[i].Coordinate.Rank).ToString();
                Console.Write($"{rankAsString} ");
            }

            string mark = Spaces[i].Mark == Mark.Empty ? " " : Spaces[i].Mark.ToString();
            
            if ((i+1) % 3 == 0)
            {
                Console.WriteLine($"[{mark}]");
                // Console.WriteLine($"[{Spaces[i].Coordinate.Value}]");
            } else {
                Console.Write($"[{mark}]");
                // Console.Write($"[{Spaces[i].Coordinate.Value}]");
            }    
        }
        
        Console.WriteLine($"   {FileLetter.A}  {FileLetter.B}  {FileLetter.C}".PadLeft(3));
    }

    public void PlaceMark(Coordinate coordinate, Mark mark)
    {
        Space space = Spaces.First(s => s.Coordinate.Value == coordinate.Value);
        space.Mark = mark;
    }
}

public class Space
{
    public Mark Mark { get; set; } = Mark.Empty;
    public Coordinate Coordinate { get; }

    public Space(Coordinate coordinate)
    {
        Coordinate = coordinate;
    }
}

/*
 * the horizontal lines of the board are called ranks
 * the vertical lines of the board are called files
 */
public class Coordinate
{
    public string Value => $"{File}{(int)Rank}";
    public FileLetter File { get; }
    public RankNumber Rank { get; }
    public Coordinate(FileLetter fileLetter, RankNumber rankNumber)
    {
        File = fileLetter;
        Rank = rankNumber;
    }
}

public enum FileLetter
{
    A,
    B,
    C
}

public enum RankNumber
{
    One = 1,
    Two = 2,
    Three = 3
}

public enum Mark
{
    Empty,
    X,
    O
}