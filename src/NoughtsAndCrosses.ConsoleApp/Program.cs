// See https://aka.ms/new-console-template for more information

using NoughtsAndCrosses.Core.Service;
using NoughtsAndCrosses.Core.Domain;

Console.WriteLine("Hello, World!");

// var playerClient = new PlayerClient();
// var consoleService = new ConsoleService(playerClient);
//
// // Create tasks
// var connectToWebSocket = playerClient.ConnectToWebSocket("ws://localhost:5148/ws");
// var monitorConsoleInput = consoleService.MonitorConsoleInput();
//
// await Task.WhenAll(
//     connectToWebSocket,
//     monitorConsoleInput
// );

var gm = new GameManager();

AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
{
    Console.WriteLine($"ProcessExit event triggered. Game running?: {gm.ListeningForInputs}");
};

gm.Run();


//
// static async Task ReadConsoleInput(PlayerClient client = null)
// {
//     while (true)
//     {
//         var input = Console.ReadLine();
//         if (input == "exit")
//         {
//             break;
//         }
//         
//         if (client != null)
//         {
//             await client.SendToWebSocket(input);
//         }
//         
//         Console.WriteLine($"You entered: {input}");
//         // Add logic to send input to WebSocket server if needed
//     }
// }

//
// var serviceProvider = new ServiceCollection()
//     .AddSingleton<ICommandService, CommandService>() // For online play
//     .AddSingleton<NPCService>() // For offline play
//     .AddSingleton<GameManager>()
//     .BuildServiceProvider();

// Console.WriteLine("Choose mode: 1 for Online, 2 for Offline");
// var choice = Console.ReadLine();
//
// if (choice == "1")
// {
//     Console.WriteLine("Connecting to server...");
//     var playerClient = new PlayerClient();
//     playerClient.ConnectAsync("ws://localhost:5000").Wait();
// }
// else
// {
//     Console.WriteLine("Starting offline game against NPC...");
//     
//     // 
//     var npcService = serviceProvider.GetService<NPCService>();
//     gameManager = new GameManager(npcService);
// }
//
// // Additional game setup and start logic...
// gameManager.NewGame();
// // Example: gameManager.PlayTurn(player);