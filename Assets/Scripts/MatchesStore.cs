using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase.Firestore;
using Firebase.Extensions;

public class MatchesStore : FirestoreStore
{
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
            Match match = new Match(snapshot);
            OnMatchUpdated(match); 
        }); 
    }
}
