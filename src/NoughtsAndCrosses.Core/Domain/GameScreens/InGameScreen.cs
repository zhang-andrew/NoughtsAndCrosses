using NoughtsAndCrosses.Core.Constant;
using NoughtsAndCrosses.Core.Domain.Exceptions;
using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain.GameScreens;

public class InGameScreen : IScreen
{
    private ConsoleService _consoleService = new ConsoleService();
    private GameManager _gameManager;

    private string[] _allowedCommands;

    public InGameScreen(GameManager gameManager)
    {
        _gameManager = gameManager;
        // _allowedCommands = new string[] { InGameCommand.Restart, GeneralCommand.Back };
    }
    
    public bool HandleInput(string input)
    {
        Board board = _gameManager.Board;

        if (board.HasWinner() || board.HasDraw())
        {
            if (input == InGameCommand.Restart)
            {
                AssignRandomMarkToPlayer();
                ComputerMovesFirstIfCrosses();
                board.Reset();
                _gameManager.Board.ShowBoard();
            }
            else
            {
                ShowEndGameMessage();
            }

            return true;
        }
        
        
        if (board.Spaces.All(s => s.Mark == Mark.Empty) && _gameManager.LocalPlayer.AssignedMark == Mark.O)
        {
            Console.WriteLine("X goes first.");
            return true;
        }

        Coordinate parsedCoordinate = Coordinate.Parse(input.ToUpper());
        _gameManager.Board.PlaceMark(parsedCoordinate, _gameManager.LocalPlayer.AssignedMark);
        
        if (_gameManager.Board.HasWinner() || _gameManager.Board.HasDraw())
        {
            _gameManager.Board.ShowBoard();
            ShowEndGameMessage();
        }
        else
        {
            var oppositeMark = _gameManager.LocalPlayer.AssignedMark == Mark.X ? Mark.O : Mark.X;
            _gameManager.Board.PlaceMarkRandomly(oppositeMark);
            _gameManager.Board.ShowBoard();
        }
        
        
        return true;
    }

    public void OnEntry()
    {
        _gameManager.Board = new Board();
        AssignRandomMarkToPlayer();
        ComputerMovesFirstIfCrosses();

        // If online game, ask server for the board state
        // Else, create a new board state
        
        // string allowedCommands = string.Join("\", \"", _allowedCommands);
        _consoleService.SystemMessage(GameScreen.InGame ,$"Type the coordinate to place your mark. Type \"back\" to go back.");
        
        _gameManager.Board.ShowBoard();
    }

    private void ComputerMovesFirstIfCrosses()
    {
        // Computer moves if player is assigned Naught
        if (_gameManager.LocalPlayer.AssignedMark == Mark.O)
        {
            _gameManager.Board.PlaceMarkRandomly(Mark.X);
        }
    }

    private void AssignRandomMarkToPlayer()
    {
        // Assign mark randomly
        Random random = new Random();
        int randomInt = random.Next(0, 2);
        Mark randomMark = randomInt == 0 ? Mark.O : Mark.X;
        _gameManager.LocalPlayer = new Player(randomMark);
        _consoleService.SystemMessage(GameScreen.InGame, $"You are assigned \"{_gameManager.LocalPlayer.AssignedMark}\"");
    }

    public void OnExit()
    {
        _consoleService.SystemMessage(GameScreen.InGame, $"Exiting \"{_gameManager.CurrentScreen}\" screen.");
    }
    
    private void ShowEndGameMessage()
    {
        // Show message based on the game result
        _consoleService.SystemMessage(GameScreen.InGame, $"Game over. \"{_gameManager.Board.GetWinner()}\" wins.\n\tType \"back\" to go back to the menu.\n\tType \"restart\" to restart the game.");
        
    }
}