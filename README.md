# Noughts And Crosses
Noughts & Crosses (Tic Tac Toe) console application built with C# and .NET. The project was developed using the Test-Driven Development (TDD) approach, applying concepts such as 'red, green, refactor' and writing behaviour-based tests.

The primary goal of this project was to practice TDD and deepen my understanding of the methodology.

## Features
- Play a game against a computer
- Host a game for another player to join `Work-in-progress`
- Join a game hosted by another player `Work-in-progress`

## Requirements
- .NET 8.0 SDK or higher

## Usage
If you have the .NET SDK installed, run the console application with the following command from the root directory:
```bash
$ dotnet run --project src/NoughtsAndCrosses.ConsoleApp
```
A build is available in the releases section of this repository.

## Folder structure
```
PROJECT_ROOT
│   # Project files
├── src
│   │   # .NET Console application code (entry point)
│   ├── ConsoleApp
│   │   # Core domain classes
│   ├── Core
│   │   # .NET WebApp code 
│   └── WebSocketServer
│   # Test files
└── tests
    │   # Console application specifications
    ├── ConsoleApp.Tests
    │   # Core domain specifications
    ├── Core.Tests
    │   # Web application specifications
    └── WebSocketServerTests.Tests
```

# TODO
1. Create a mock for the WebSocketServer
2. Clear the console screen for the clientPlayer after making their move