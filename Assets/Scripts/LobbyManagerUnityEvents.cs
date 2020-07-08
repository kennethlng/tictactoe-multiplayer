using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class LobbyManagerUnityEvents : MonoBehaviour
{
    public UnityEvent OnMatchSelected = new UnityEvent(); 

    private void OnEnable()
    {
        LobbyManager.OnMatchSelected += HandleMatchSelected;
    }

    private void OnDisable()
    {
        LobbyManager.OnMatchSelected -= HandleMatchSelected;
    }

    private void HandleMatchSelected()
    {
        if (OnMatchSelected != null)
            OnMatchSelected.Invoke(); 
    }
}
