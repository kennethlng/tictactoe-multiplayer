using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore; 

public class Match
{
    public string id;
    public bool isActive;
    public string winner;
    public string turn; 
    public static CollectionReference collectionRef = FirebaseFirestore.DefaultInstance.Collection(Constants.MATCHES);
    public List<string> marks = new List<string>();
    public List<Player> players = new List<Player>(); 

    public Match(DocumentSnapshot documentSnapshot)
    {
        Dictionary<string, object> data = documentSnapshot.ToDictionary(); 

        this.id = documentSnapshot.Id;
        this.isActive = (bool)data[Constants.IS_ACTIVE];
        this.turn = (string)data[Constants.TURN]; 
        this.winner = (string)data[Constants.WINNER];
        this.players.Add(new Player((string)data["playerO"], Constants.MARK_O));
        this.players.Add(new Player((string)data["playerX"], Constants.MARK_X));
        this.marks.Add((string)data["mark0"]);
        this.marks.Add((string)data["mark1"]);
        this.marks.Add((string)data["mark2"]);
        this.marks.Add((string)data["mark3"]);
        this.marks.Add((string)data["mark4"]);
        this.marks.Add((string)data["mark5"]);
        this.marks.Add((string)data["mark6"]);
        this.marks.Add((string)data["mark7"]);
        this.marks.Add((string)data["mark8"]);
    }
}
