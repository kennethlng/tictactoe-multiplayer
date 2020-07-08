using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore; 

public class Queue
{
    public string id; 
    public string userId;
    public bool isActive;
    public static CollectionReference collectionRef = FirebaseFirestore.DefaultInstance.Collection(Constants.QUEUES);

    public Queue(DocumentSnapshot documentSnapshot)
    {
        Dictionary<string, object> data = documentSnapshot.ToDictionary();

        this.id = documentSnapshot.Id;
        this.userId = (string)data[Constants.USER_ID];
        this.isActive = (bool)data[Constants.IS_ACTIVE]; 
    }
}
