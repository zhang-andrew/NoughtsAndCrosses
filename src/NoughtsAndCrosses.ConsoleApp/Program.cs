// See https://aka.ms/new-console-template for more information
using NoughtsAndCrosses.ConsoleApp.Domain;

Console.WriteLine("Hello, World!");

var consoleAppClient = new Client();

// Create tasks
var webSocketTask = consoleAppClient.ConnectToWebSocket("ws://localhost:5148/ws");
// var randomTask = consoleAppClient.SendToWebSocket("testtt");
var consoleInputTask = ReadConsoleInput(consoleAppClient);

await Task.WhenAll(
    webSocketTask,
    // randomTask,
    consoleInputTask
);


static async Task ReadConsoleInput(Client client = null)
{
    while (true)
    {
        var input = Console.ReadLine();
        if (input == "exit")
        {
            break;
        }
        
        if (client != null)
        {
            await client.SendToWebSocket(input);
        }
        
        Console.WriteLine($"You entered: {input}");
        // Add logic to send input to WebSocket server if needed
    }
}

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