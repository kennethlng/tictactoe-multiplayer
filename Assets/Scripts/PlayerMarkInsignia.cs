using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth; 

public class PlayerMarkInsignia : MonoBehaviour
{
    public Image turnIndicatorImage;
    public Image xImage;
    public Image oImage; 
    public Text text; 
    public Player player;
    private Color defaultColor = Color.clear;
    private Color turnColor = Color.blue;

    private void OnEnable()
    {
        GameManager.OnTurnChanged += HandleGameChangeTurn;
    }

    private void OnDisable()
    {
        GameManager.OnTurnChanged -= HandleGameChangeTurn;
    }

    private void Start()
    {
        turnIndicatorImage.color = defaultColor; 
    }

    public void Setup(Player player)
    {
        this.player = player;

        string authUserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        text.text = authUserId == player.id ? "You" : "Opponent";

        bool isOMark = player.mark == Constants.MARK_O;
        oImage.gameObject.SetActive(isOMark);
        xImage.gameObject.SetActive(!isOMark); 
    }

    private void HandleGameChangeTurn(string userId)
    {
        if (player == null) return;
        turnIndicatorImage.color = userId == player.id ? turnColor : defaultColor; 
    }
}
