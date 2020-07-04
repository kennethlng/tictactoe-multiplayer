using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private MatchesStore matchesStore; 

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        matchesStore = new MatchesStore();
        matchesStore.ListenMatch("isXFXtKslUfhIbAtLwb1");
        matchesStore.OnMatchUpdated += MatchesStore_OnMatchUpdated;
    }

    private void MatchesStore_OnMatchUpdated(Match match)
    {
        Debug.Log(match);
    }
}
