using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; 
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

public class MatchesStore : FirestoreStore
{
    private Match _match = null;
    public Match match { get { return _match; } }
    public delegate void OnMatchUpdatedDelegate(Match match);
    public event OnMatchUpdatedDelegate OnMatchUpdated;

    public void ListenMatches()
    {

    }

    public void ListenMatch(string matchId)
    {
        DocumentReference docRef = Match.collectionRef.Document(matchId);
        docRef.Listen(snapshot =>
        {
            Debug.Log("Match updated");
            Match newMatch = new Match(snapshot);
            _match = newMatch; 
            OnMatchUpdated(match); 
        }); 
    }

    public Task GetMatch(string matchId)
    {
        DocumentReference docRef = Match.collectionRef.Document(matchId);
        return docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            var snapshot = task.Result;

            if (snapshot.Exists)
            {
                Match newMatch = new Match(snapshot);
                _match = newMatch;
            }
            else
            {
                Debug.Log("Snapshot doesn't exist"); 
            }
        });
    }

    public Task UpdateMatch(Match match)
    {
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

        return docRef.SetAsync(updatedMatch, SetOptions.MergeAll); 
    }
}
