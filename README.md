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

### Signing In

For simplicity's sake, I used Firebase Authentication's anonymous sign-in.  

### Matchmaking

When the `LobbyScene` opens, the `LobbyManager` starts listening for available matches for the player to play. 

```c#
//  MatchesStore.cs

public void ListenMatches()
{
    //  Get the signed-in user's ID
    string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId; 

    //  Listen for active matches where the user is a player 
    Query query = db.collection("matches").WhereEqualTo(userId, true).WhereEqualTo("isActive", true);
    listener = query.Listen(snapshot =>
    {
        List<Match> matches = new List<Match>();
        foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
        {
            Match match = new Match(documentSnapshot);
            matches.Add(match);
        }
        
        //  Broadcast event with the list of matches
        OnMatchesUpdated?.Invoke(matches); 
    });
}
```

If a match is available it will be populated on the screen, and the player can click on it to initiate the game. The match ID is saved to a `ScriptableObject` so that it can be accessed by the `GameManager` in the `GameScene`. 

If there are no matches available, the player can submit a matchmaking request (create a `queue`). 

```c#
//  QueuesStore.cs

public Task CreateQueue()
{
    //  Get the signed-in user's ID
    string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId; 

    //  Create a new queue document
    Dictionary<string, object> queue = new Dictionary<string, object>
    {
        { "userId", userId }
    };

    return db.collection("queues").AddAsync(queue).ContinueWithOnMainThread(task =>
    {
        DocumentReference newDocRef = task.Result;
        Debug.Log("Added queue document with ID: " + newDocRef.Id);
        
        //  Broadcast event
        OnQueueCreated(); 
    });
}
```

### Gameplay

When the `GameScene` opens, the `GameManager` starts listening for the match with the match ID provided by the lobby. Each time the `match` document in the Firestore database collection is updated, the match object on the client side is also updated to reflect the latest updates. 

```c#
//  MatchesStore.cs

public void ListenMatch(string matchId)
{
    DocumentReference docRef = db.collection("matches").Document(matchId);
    listener = docRef.Listen(snapshot =>
    {
        Match match = new Match(snapshot);
        
        //  Broadcast event with the updated match
        OnMatchUpdated?.Invoke(match);
    }); 
}
```

Each time the match is updated, the `GameManager` broadcasts events for the grid space marks and for whose turn it is. 

```c#
private void OnMatchUpdated(Match match)
{
    if (match == null) return;

    this.match = match;

    //  If the game is no longer active, don't run any events or game logic
    if (!match.isActive)
    {
        //  If the game is no longer active, run the game over logic
        GameOver();
        return;
    }

    //  Broadcast event for the updated grid space marks
    OnMarksUpdated?.Invoke(match.marks); 
    
    //  Broadcast event for which player's turn it is
    OnTurnChanged?.Invoke(match.turn);
}
```

### Events

#### `OnMarksUpdated` 

Passes a List<string> of marks corresponding to the 9 grid spaces. The mark buttons on the 9 grid spaces listen for this event and change their mark to "O" or "X" accordingly when the marks are updated. 
    
#### `OnTurnChanged`

Passes the user ID of the player whose turn it is to place a mark on the grid. The mark buttons on the 9 grid spaces listen for this event and disable themselves when it's not the signed-in user's turn. The `PlayerIndicatorPanel` on the UI also listens for this event to tell the player whose turn it is. 

### Database

The Firestore database has 2 collections: `queues` and `matches`. 

#### `queue`

A `queue` document has the following fields:

```json
"userId": "0cj3jkdflj2ldkfjd"
"isActive": true
"createdOn": "July 6, 2020 at 12:14:45 PM UTC-7"
```

Field | Type | Description 
| --- | --- | --- |
`userId` | `string` | Identifies the player who created the matchmaking request. 
`isActive` | `bool` | Determine whether the matchmaking request is currently active. This is set by the Cloud Functions backend. 
`createdOn` | `timestamp` | This is set by the Cloud Functions backend. 

#### `match`

A `match` document: 

```json
{
    "isActive": true,
    "playerO": "T2ekijVZMgOhVA48z86Vxam4XZm2",
    "playerX": "xTES8vbX84NbqmX10RGBScWt1Jf2",
    "players": {
        "xTES8vbX84NbqmX10RGBScWt1Jf2": true,
        "T2ekijVZMgOhVA48z86Vxam4XZm2": true
    },
    "turn": "T2ekijVZMgOhVA48z86Vxam4XZm2",
    "winner": "",
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
    },
    "createdOn": "July 6, 2020 at 01:25:42 PM UTC-7",
    "modifiedOn": "July 8, 2020 at 10:05:31 PM UTC-7",
}
```

Field | Type | Description
--- | --- | --- 
`isActive` | `bool` | Determines whether the match is currently active. A match is still active when the 2 players are playing. A match is set inactive when the game ends. 
`turn` | `string` | The user ID of the player whose turn it is. This field helps the clients determine whose turn it is and whether or not to disable input from the player. The Cloud Functions backend determines whose turn it is. 
`playerO` | `string` | The user ID of the player whose mark is "O".
`playerX` | `string` | The user ID of the player whose mark is "X".
`winner` | `string` | The user ID of the winning player, if there is a winner. If there is no winner, this field is left blank. 
`players` | `object` | This field is used on the client side for querying matches that the signed-in user is a member of. 
`marks` | `object` | An object mapping representing the 9 grid spaces in a TicTacToe game. This field is updated each time a player places a new mark on the grid. 

## Contact

If you have any questions, message me on [Twitter](https://twitter.com/kennethlng) or email me at hello@kennethlng.com.
