using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AuthenticationManagerUnityEvents : MonoBehaviour
{
    public UnityEvent OnSignedIn = new UnityEvent();
    public UnityEvent OnSignedOut = new UnityEvent(); 

    private void OnEnable()
    {
        AuthenticationManager.OnSignedIn += HandleSignIn;
        AuthenticationManager.OnSignedOut += HandleSignOut;
    }

    private void OnDisable()
    {
        AuthenticationManager.OnSignedIn -= HandleSignIn;
        AuthenticationManager.OnSignedOut -= HandleSignOut;
    }

    private void HandleSignIn()
    {
        if (OnSignedIn != null)
            OnSignedIn.Invoke(); 
    }

    private void HandleSignOut()
    {
        if (OnSignedOut != null)
            OnSignedOut.Invoke(); 
    }
}
