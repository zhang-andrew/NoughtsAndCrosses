namespace NoughtsAndCrosses.Core.Domain.GameScreens;

public interface IScreen
{
    public bool HandleInput(string input);
    public void OnEntry();
    public void OnExit();
}