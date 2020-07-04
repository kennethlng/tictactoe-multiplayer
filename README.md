# Multiplayer TicTacToe

## Installation

## How to Play

Click on the "Find Match" button to initiate a matchmaking request. This automatically places you in a queue until another player is available. Once a match is found, the game begins. 

TicTacToe is a simple, turn-based strategy game where you must outwit the opponent. Players take turns placing a mark on the 3x3 grid. The first player to have a vertical, horizontal, or diagonal line of marks wins the game. 

## Stack 

Firebase, Unity

## How It Works

### 1. Sign-in

For the sake of simplicity, I only enabled anonymous sign-in. When the game starts, users are automatically logged in as anonymous users based on their device.

### 2. Matchmaking

When a player presses the "Find Match" button, a `queue` document is created in the Firestore `queues` collection. The player's client immediately begins listening for new `match` documents.  

Each `queue` document is mapped to the player UID that initiated the request and contains an `isActive` field to indicate whether the queue is currently active. Whenever a `queue` document is created, the Cloud Functions checks for other `queue` docs that are currently active. 

If 2 active queues are available, a `match` is created for the 2 players, the queues are deactivated, and the players' clients start a game. 

### 3. Playing

Since there are a finite number of moves for each TicTacToe game, the entire grid is saved as an object in the `match` document. 

```
```

### 4. Ending

When a winner is declared, the `match` document is updated with the ID of the winning player.

```C#
DocumentReference matchRef = db.Collection("matches").Document("match-id");
Dictionary<string, object> updates = new Dictionary<string, object>
{
        { "isActive", false },
        { "winner", winning-player-id }
};

matchRef.UpdateAsync(updates).ContinueWithOnMainThread(task => {
        Debug.Log("Updated the isActive and winner fields of the match-id document in the matches collection.");
});
```
