// See https://aka.ms/new-console-template for more information

using NoughtsAndCrosses.Core.Domain;

var gm = AppManager.Instance;

// AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
// {
//     Console.WriteLine($"ProcessExit event triggered. Game running?: {gm.IsListeningForInputs}");
// };

gm.Run();