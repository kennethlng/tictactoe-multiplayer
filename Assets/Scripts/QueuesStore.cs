using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Auth; 
using System.Threading.Tasks;

public class QueuesStore
{
    public delegate void OnQueueCreatedDelegate();
    public event OnQueueCreatedDelegate OnQueueCreated; 

    public Task CreateQueue()
    {
        Debug.Log("Create queue"); 
        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId; 

        Dictionary<string, object> queue = new Dictionary<string, object>
        {
            { Constants.USER_ID, userId }
        };

        return Queue.collectionRef.AddAsync(queue).ContinueWithOnMainThread(task =>
        {
            DocumentReference newDocRef = task.Result;
            Debug.Log("Added queue document with ID: " + newDocRef.Id);
            OnQueueCreated(); 
        });
    }
}
