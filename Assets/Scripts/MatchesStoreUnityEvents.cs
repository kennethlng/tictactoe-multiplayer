using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class MatchesStoreUnityEvents : MonoBehaviour
{
    public UnityEvent OnMatchUpdate = new UnityEvent();
    public UnityEvent OnMatchUpdated = new UnityEvent();
    public UnityEvent OnMatchesUpdated = new UnityEvent();

    private void OnEnable()
    {
        MatchesStore.OnMatchUpdate += HandleMatchUpdate;
        MatchesStore.OnMatchUpdated += HandleMatchUpdated;
        MatchesStore.OnMatchesUpdated += HandleMatchesUpdated;
    }

    private void OnDisable()
    {
        MatchesStore.OnMatchUpdate -= HandleMatchUpdate;
        MatchesStore.OnMatchUpdated -= HandleMatchUpdated;
        MatchesStore.OnMatchesUpdated -= HandleMatchesUpdated;
    }

    private void HandleMatchUpdated(Match match)
    {
        if (OnMatchUpdated != null) OnMatchUpdated.Invoke(); 
    }

    private void HandleMatchesUpdated(List<Match> matches)
    {
        if (OnMatchesUpdated != null) OnMatchesUpdated.Invoke(); 
    }

    private void HandleMatchUpdate()
    {
        if (OnMatchUpdate != null) OnMatchUpdate.Invoke(); 
    }

    
}
