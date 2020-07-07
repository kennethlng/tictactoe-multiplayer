using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; 
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

public class MatchesStore : FirestoreStore
{
    public delegate void OnMatchUpdateDelegate();
    public event OnMatchUpdateDelegate OnMatchUpdate;  
    public delegate void OnMatchUpdateErrorDelegate();
    public event OnMatchUpdateErrorDelegate OnMatchUpdateError;
    public delegate void OnMatchUpdatedDelegate(Match match);
    public event OnMatchUpdatedDelegate OnMatchUpdated;

    public void ListenMatch(string matchId)
    {
        DocumentReference docRef = Match.collectionRef.Document(matchId);
        docRef.Listen(snapshot =>
        {
            Debug.Log("Match updated");
            Match newMatch = new Match(snapshot);
            OnMatchUpdated(newMatch); 
        }); 
    }

    public Task UpdateMatch(Match match)
    {
        OnMatchUpdate();

        DocumentReference docRef = Match.collectionRef.Document(match.id);
        Dictionary<string, object> updatedMatch = new Dictionary<string, object>
        {
            { "isActive", match.isActive },
            { "mark0", match.marks[0] },
            { "mark1", match.marks[1] },
            { "mark2", match.marks[2] },
            { "mark3", match.marks[3] },
            { "mark4", match.marks[4] },
            { "mark5", match.marks[5] },
            { "mark6", match.marks[6] },
            { "mark7", match.marks[7] },
            { "mark8", match.marks[8] }
        };

        return docRef.SetAsync(updatedMatch, SetOptions.MergeAll).ContinueWithOnMainThread(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
                OnMatchUpdateError(); 
            } 
        }); 
    }
}
