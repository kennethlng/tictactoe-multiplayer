using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore; 

public class Match
{
    public string id;
    public bool isActive;
    public string winner;
    public static CollectionReference collectionRef = FirebaseFirestore.DefaultInstance.Collection(Constants.MATCHES);

    public Match(DocumentSnapshot documentSnapshot)
    {
        Dictionary<string, object> data = documentSnapshot.ToDictionary(); 

        this.id = documentSnapshot.Id;
        this.isActive = (bool)data[Constants.IS_ACTIVE];
        this.winner = (string)data[Constants.WINNER]; 
    }
}
