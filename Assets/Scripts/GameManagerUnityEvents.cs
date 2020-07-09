using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class GameManagerUnityEvents : MonoBehaviour
{
    public UnityEvent OnGameQuit = new UnityEvent(); 

    private void OnEnable()
    {
        GameManager.OnGameQuit += HandleGameQuit;
    }

    private void OnDisable()
    {
        GameManager.OnGameQuit -= HandleGameQuit;
    }

    private void HandleGameQuit()
    {
        if (OnGameQuit != null)
            OnGameQuit.Invoke(); 
    }
}
