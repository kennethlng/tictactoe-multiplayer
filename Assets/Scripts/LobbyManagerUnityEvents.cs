using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class LobbyManagerUnityEvents : MonoBehaviour
{
    public UnityEvent OnMatchFind = new UnityEvent(); 
    public UnityEvent OnMatchSelected = new UnityEvent();

    private void OnEnable()
    {
        LobbyManager.OnMatchFind += HandleMatchFind;
        LobbyManager.OnMatchSelected += HandleMatchSelected;
    }

    private void OnDisable()
    {
        LobbyManager.OnMatchFind -= HandleMatchFind;
        LobbyManager.OnMatchSelected -= HandleMatchSelected;
    }

    private void HandleMatchFind()
    {
        if (OnMatchFind != null) OnMatchFind.Invoke();
    }

    private void HandleMatchSelected()
    {
        if (OnMatchSelected != null) OnMatchSelected.Invoke(); 
    }
}
