# Battleship-API

API for battleship game of 2 bots playing against each other created with .NET 6.

## Task

Based on the rules of Battleship board game (https://en.wikipedia.org/wiki/Battleship_(game)) implement a program which randomly places ships on two boards and simulates the gameplay between 2 players.

## Game Rules

- Playing board is of size 10x10 spots.
- Each player have 5 ship -> 1 x Destroyer (2 spots long), 1 x Cruiser (3 spots long), 1 x Battleship (4 spots long), 1 x Carrier (5 spots long).
- Ships can be placed horizontally or vertically.
- Ships can be placed only within size of the board and can't overlap, but they can touch eachother.
- Every move, players change turns - no matter of outcome(miss, hit etc.).
- Both players play 100% randomly.

## Backend

Because there are two bots playing against each other, I assumed that there would always be 1 match at a time (not several at the same time), so two tables are created to store both players' boards (coordinates). If there would be multiple matches taking place at the same time, it would probably be necessary to create a table for the matches, one coordinates table with the FK of the matches, and so on. I adopted the simplest version. 

The backend is split into a repositories layer that has the database injected and sends the appropriate queries to it through methods like "Get board for a single player", "Generate hits' coordinates", "Update database with new coordinates" and also "Save changes to the database" and "Clear the database". The service layer has injected this repository and performs the required additional logic to return a response in the form of an object {"List of coordinates with status, whether hit and whether ship"; "whether any player won, if so, which one"; "for which player is this response"} after hitting the appropriate endpoints from the frontend.

There are 3 enpoints in the controller - "create empty board for player (dependent on id [1 or 2] from front)"; "Make a move for a player (dependent on id [1 or 2] from front)"; "Clear both tables in database".

So all of the logic takes place on the backend side and sends information to the frontend about the updated state of the board for a given player.
