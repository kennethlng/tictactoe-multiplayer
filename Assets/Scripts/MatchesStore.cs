using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; 
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Auth; 

public class MatchesStore : FirestoreStore
{
    private ListenerRegistration listener; 
    public delegate void OnMatchUpdateDelegate();
    public event OnMatchUpdateDelegate OnMatchUpdate;  
    public delegate void OnMatchUpdateErrorDelegate();
    public event OnMatchUpdateErrorDelegate OnMatchUpdateError;
    public delegate void OnMatchUpdatedDelegate(Match match);
    public event OnMatchUpdatedDelegate OnMatchUpdated;
    public delegate void OnMatchesUpdatedDelegate(List<Match> matches);
    public event OnMatchesUpdatedDelegate OnMatchesUpdated; 

    public void ListenMatches()
    {
        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId; 

        Query query = Match.collectionRef.WhereEqualTo(userId, true).WhereEqualTo(Constants.IS_ACTIVE, true);
        listener = query.Listen(snapshot =>
        {
            Debug.Log("Matches updated");
            List<Match> matches = new List<Match>();
            foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
            {
                Match match = new Match(documentSnapshot);
                matches.Add(match);
            }
            OnMatchesUpdated?.Invoke(matches); 
        });
    }

    public void ListenMatch(string matchId)
    {
        DocumentReference docRef = Match.collectionRef.Document(matchId);
        listener = docRef.Listen(snapshot =>
        {
            Debug.Log("Match updated");
            Match newMatch = new Match(snapshot);
            OnMatchUpdated?.Invoke(newMatch);
        }); 
    }

    public Task UpdateMatch(Match match)
    {
        OnMatchUpdate?.Invoke();

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
                OnMatchUpdateError?.Invoke(); 
            } 
        }); 
    }

    public void Unlisten()
    {
        listener.Stop(); 
    }
}
