using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class SignIn : MonoBehaviour
{
    public Button signInButton;

    private void OnEnable()
    {
        AuthenticationManager.OnSigningIn += HandleSigningIn;
        AuthenticationManager.OnSignInFailed += HandleSignInFailed;
        signInButton.interactable = true; 
    }

    private void OnDisable()
    {
        AuthenticationManager.OnSigningIn -= HandleSigningIn;
        AuthenticationManager.OnSignInFailed -= HandleSignInFailed;
        signInButton.interactable = false; 
    }

    private void Start()
    {
        signInButton.onClick.AddListener(OnSignInButtonClicked);
    }

    private void HandleSigningIn()
    {
        signInButton.interactable = false; 
    }

    private void HandleSignInFailed()
    {
        signInButton.interactable = true;
    }

    private void OnSignInButtonClicked()
    {
        AuthenticationManager.Instance.SignInAnonymously(); 
    }


}
