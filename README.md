# Multiplayer TicTacToe Using Unity and Firebase

## Installation

### 1. Download and unzip the project

Download the Xcode project and unzip it onto your computer. 

### 2. Open the project in Xcode

Launch Xcode and you will be prompted to create or open a project. Select the option **Open a Project or File**. Locate the project folder and open `Unity-iPhone.xcworkspace`. 

### 3. Fix dependencies

Before building the project onto your iPhone, you may experience some build errors. Remember to update the cocoapods by opening Terminal, locating the project folder, and entering `pod update`. 

### 4. Connect your iPhone 

Connect your iPhone and choose your phone in the list of build devices. 

### 4. Build

Press **Build** and Xcode will begin building the game onto your iPhone. Once it is finished building, the game will automatically open on your phone. 

## How to Play

Click on the "Find Match" button to initiate a matchmaking request. This automatically places you in a queue until another player makes a matchmaking request and a match is created. A list of available matches will be shown on the screen. To begin the game, click on one of the available matches.

TicTacToe is a simple, turn-based strategy game where you must outwit the opponent. When the game begins, the upper panel will indicate whether you have been assigned the "X" or "O" mark and whose turn it is each round (as judged by the blue highlight). Players take turns placing a mark on the 3x3 grid. When it is your turn, tap on any of the empty grid spaces to place your mark. The first player to have a vertical, horizontal, or diagonal line of marks wins the game. 

Once the game is over, you can return to the lobby to find another match. 
