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
    
    public void HandleInputs(string input)
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
    }

    public void OnEntry()
    {
        _consoleService.SystemMessage(GameScreen.InGameScreen ,"Type \"back\" to go back to the menu.");
    }

    public void OnExit()
    {
        _consoleService.SystemMessage(GameScreen.InGameScreen, $"Exiting \"{_gameManager.CurrentScreen}\" screen.");
    }
}