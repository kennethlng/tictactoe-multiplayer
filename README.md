# tictactoe-multiplayer

This is a cross-platform game prototype leveraging Unity and Firebase to create an online, multiplayer turn-based strategy game. 

> This repository is only for the client-side game. Please see the repository [tictactoe-multiplayer-server](https://github.com/kennethlng/tictactoe-multiplayer-server) to see the Firebase Cloud Functions server that handles the backend game logic (determing whose turn it is, verifying win conditions). 

## Stack

I used Unity for the client-side game, Firebase Firestore for the database, and Firebase Cloud Functions for handling backend logic. Since TicTacToe is a turn-based stategy game, players could technically take turns at anytime. No real-time multiplayer networking is needed, so I opted to use Firestore as the serverless database. The game clients can read and write data directly to Firestore, and Cloud Functions trigger functions will respond to changes in the data and handle the matchmaking and game-winning logic. 

## Installation

1. Open the project in Unity. 
2. Build the game for iOS. Unity will create an Xcode project.
3. Open the Xcode project. Make sure to open `Unity-iPhone.xcworkspace` and not `Unity-iPhone.xcodeproj`.
4. Connect your iPhone. 
5. Select your phone in the list of available devices and build the game. 

While building the game in Xcode you may experience some build errors for missing dependencies. Remember to update the cocoapods by opening Terminal, locating the project folder for the Xcode project, and doing `pod update`. 

## How to Play

Click on the "Find Match" button to initiate a matchmaking request. This automatically places you in a queue until another player makes a matchmaking request and a match is created. A list of available matches will be shown on the screen. To begin the game, click **Play** on one of the available matches.

When the game begins, the upper panel will indicate whether you have been assigned the "X" or "O" mark and whose turn it is each round (as judged by the blue underline). Players take turns placing a mark on the 3x3 grid. When it is your turn, tap on any of the empty grid spaces to place your mark. The first player to have a vertical, horizontal, or diagonal line of marks wins the game. 

Once the game is over, you can return to the lobby to find another match by pressing **Play again**. 

## How It Works

### Database

A `Match` document: 

```json
"isActive": true
"playerO": "T2ekijVZMgOhVA48z86Vxam4XZm2"
"playerX": "xTES8vbX84NbqmX10RGBScWt1Jf2"
"turn": "T2ekijVZMgOhVA48z86Vxam4XZm2"
"winner": ""
"marks": {
    "0": "",
    "1": "O",
    "2": "",
    "3": "X",
    "4": "O",
    "5": "",
    "6": "X",
    "7": "",
    "8": "O"
}
"createdOn": "July 6, 2020 at 01:25:42 PM UTC-7"
"modifiedOn": "July 8, 2020 at 10:05:31 PM UTC-7"
```
