using NoughtsAndCrosses.Core.Enum;
using NoughtsAndCrosses.Core.Service;

namespace NoughtsAndCrosses.Core.Domain.GameScreens;

public class InGameScreen : IScreen
{
    private ConsoleService _consoleService = new ConsoleService();
    private GameManager _gameManager;

    public InGameScreen(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    
    public bool HandleInput(string input)
    {
        Board board = _gameManager.Board;
        
        if (board.HasWinner() || board.HasDraw())
        {
            if (input == "restart")
            {
                board.Reset();
                _gameManager.Board.ShowBoard();
            }
            else
            {
                ShowEndGameMessage();
            }

            return true;
        }
        
        Coordinate parsedCoordiante = Coordinate.Parse(input.ToUpper());
        _gameManager.Board.PlaceMark(parsedCoordiante, _gameManager.LocalPlayer.AssignedMark);
        _gameManager.Board.ShowBoard();
        
        if (_gameManager.Board.HasWinner() || _gameManager.Board.HasDraw())
        {
            ShowEndGameMessage();
        }
        
        return true;
    }

    public void OnEntry()
    {
        // If local player is not assigned, assign a mark
        if (_gameManager.LocalPlayer == null)
        {
            // Assign mark randomly
            Random random = new Random();
            int randomInt = random.Next(0, 2);
            Mark randomMark = randomInt == 0 ? Mark.O : Mark.X;
        
            _gameManager.LocalPlayer = new Player(randomMark);
        }
        
        // If online game, ask server for the board state
        // Else, create a new board state
        _gameManager.Board = new Board();
        
        _consoleService.SystemMessage(GameScreen.InGame ,"Type \"back\" to go back to the menu.");
        
        _gameManager.Board.ShowBoard();
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