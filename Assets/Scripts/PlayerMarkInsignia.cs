using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerMarkInsignia : MonoBehaviour
{
    private Image image;
    public Player player;

    private void Awake()
    {
        image = gameObject.GetComponent<Image>();
    }

    public void Activate(bool isActivated)
    {
        if (isActivated)
        {
            image.color = Color.blue; 
        } else
        {
            image.color = Color.gray;
        }
    }
}
