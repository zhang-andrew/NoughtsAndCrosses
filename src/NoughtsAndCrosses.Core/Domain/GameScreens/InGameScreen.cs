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
        if (_gameManager.LocalPlayer == null)
        {
            // Assign mark randomly
            Random random = new Random();
            int randomInt = random.Next(0, 2);
            Mark randomMark = randomInt == 0 ? Mark.O : Mark.X;
        
            _gameManager.LocalPlayer = new Player(randomMark);
        }
        
        Coordinate parsedCoordiante = Coordinate.Parse(input.ToUpper());
        _gameManager.BoardState.Board.PlaceMark(parsedCoordiante, _gameManager.LocalPlayer.AssignedMark);
        _gameManager.BoardState.Board.ShowBoard();
        
        if (_gameManager.BoardState.Board.HasWinner() )
        {
            _consoleService.SystemMessage(GameScreen.InGame, "Game over. <insert> wins. Type \"back\" to go back to the menu.");
        }
        
        if (_gameManager.BoardState.Board.HasDraw())
        {
            _consoleService.SystemMessage(GameScreen.InGame, "Game over. Type \"back\" to go back to the menu.");
        }

        return true;
    }

    public void OnEntry()
    {
        // If online game, ask server for the board state
        // Else, create a new board state
        _gameManager.BoardState = new BoardState();
        
        _consoleService.SystemMessage(GameScreen.InGame ,"Type \"back\" to go back to the menu.");
        
        _gameManager.BoardState.Board.ShowBoard();
    }

    public void OnExit()
    {
        _consoleService.SystemMessage(GameScreen.InGame, $"Exiting \"{_gameManager.CurrentScreen}\" screen.");
    }
}