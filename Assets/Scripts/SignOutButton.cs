using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignOutButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>(); 
    }

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        AuthenticationManager.Instance.SignOut(); 
    }
}
