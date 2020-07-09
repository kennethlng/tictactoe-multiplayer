using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingIndicator : MonoBehaviour
{
    public Image icon;
    public Text text;

    private void Start()
    {
        icon.enabled = false;
        text.enabled = false; 
    }

    public void Activate(bool isActivated)
    {
        icon.enabled = isActivated;
        icon.enabled = isActivated; 
    }
}
