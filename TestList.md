# TestList

- [x] The game starts with a 3x****3 square grid, containing 9 available unoccupied spaces.
- [x] Each space in the grid can only be occupied by either an "X" or an "O".
- [x] Only an unoccupied space in the grid can be occupied.
- [x] An occupied space in the grid cannot be overwritten once occupied.
- [x] If 3 spaces are occupied by the same symbol, AND linear to each other, either diagonally, horizontally or vertically, then the game ends and the owner of that symbol is victorious.
- [x] If all 9 spaces are occupied and no winner has been announced, then a draw happens.
- [ ] Players then take turns and place an "X" or an "O" in unoccupied spaces.
- [ ] Each turn has a time limit of 15 seconds.
- [ ] A user can either vs a computer or a another user.
- [ ] Users can connect to other users via the lobby.

- [x] App recognises coordinates in the format of "a1", "B2", "c3", etc.
- [ ] App can only recognise 2 players.
- [ ] App recognises who is the current player's turn.

# ConsoleApp Tests
- [x] Provide options to start a new game locally (against a computer/against another player) or online (against another player)
- [ ] Persist game state locally (client-side)
- [x] Be able to consume Console commands and be connected to a WebSocket server
- [ ] Receive WebSocket messages and react accordingly
- 
- [x] When a winner is announced, the game should end and the winner should be displayed
- [ ] When a draw is announced, the game should end and a draw should be displayed
- [x] When a player makes a move, the game state should be updated and displayed
- [ ] When a player makes a move, the game state should be updated and displayed (Online)
- [x] When the game has ended, options to start a new game should be displayed
- [ ] When the game has ended, all coordinate commands should be disabled
- [ ] If a rematch is requested, the game should reset and start again

# WebSocketServer Tests
- [ ] Handle multiple connections
- [ ] Handle multiple concurrent lobbies/games
- [ ] Differentiate between Clients (player's consoleApp clients)
- [ ] Persist game state in memory (on the server)
- [ ] Player moves (placing an "X" or an "O") should update the lobby/game state and then broadcast the updated state to all clients in the lobby/game
 

- [ ] Decouple consoleApp input commands from Core GameManager logic


# Todos
- [ ] Exit command should prompt user to confirm exit.
- [ ] Back command should only be available in HostGameScreen and JoinGameScreen.