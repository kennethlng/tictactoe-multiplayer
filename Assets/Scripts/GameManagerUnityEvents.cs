using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class GameManagerUnityEvents : MonoBehaviour
{
    public UnityEvent OnMarksUpdate = new UnityEvent();
    public UnityEvent OnMarksUpdated = new UnityEvent(); 
    public UnityEvent OnGameQuit = new UnityEvent();

    private void OnEnable()
    {
        GameManager.OnGameQuit += HandleGameQuit;
        GameManager.OnMarksUpdate += HandleMarksUpdate;
        GameManager.OnMarksUpdated += HandleMarksUpdated;
    }

    private void OnDisable()
    {
        GameManager.OnGameQuit -= HandleGameQuit;
        GameManager.OnMarksUpdate -= HandleMarksUpdate;
        GameManager.OnMarksUpdated -= HandleMarksUpdated;
    }

    private void HandleGameQuit()
    {
        if (OnGameQuit != null)
            OnGameQuit.Invoke(); 
    }

    private void HandleMarksUpdate()
    {
        if (OnMarksUpdate != null) OnMarksUpdate.Invoke(); 
    }

    private void HandleMarksUpdated(List<string> marks)
    {
        if (OnMarksUpdated != null) OnMarksUpdated.Invoke(); 
    }
}
