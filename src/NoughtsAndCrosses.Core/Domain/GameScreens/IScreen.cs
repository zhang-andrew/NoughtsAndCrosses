namespace NoughtsAndCrosses.Core.Domain.GameScreens;

public interface IScreen
{
    public void HandleInputs(string input);
    public void OnEntry();
    public void OnExit();
}