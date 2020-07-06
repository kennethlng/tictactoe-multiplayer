using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Auth; 

public class GameManager : MonoBehaviour
{
    private MatchesStore matchesStore;
    private Match match; 
    public MarkButton[] markButtons;
    public PlayerMarkInsignia[] playerMarkInsignias;

    // Start is called before the first frame update
    //async void Start()
    //{
    //    await matchesStore.GetMatch("isXFXtKslUfhIbAtLwb1");

    //    //matchesStore.ListenMatch("isXFXtKslUfhIbAtLwb1");
    //    //matchesStore.OnMatchUpdated += MatchesStore_OnMatchUpdated;
    //}

    private void Awake()
    {
        matchesStore = new MatchesStore(); 
    }

    private void Start()
    {
        //matchesStore.GetMatch("isXFXtKslUfhIbAtLwb1");
        matchesStore.ListenMatch("isXFXtKslUfhIbAtLwb1");
        matchesStore.OnMatchUpdated += MatchesStore_OnMatchUpdated;
        Setup(); 
    }

    private void Setup()
    {
        Debug.Log(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        SubscribeToMarkButtons(); 
    }

    private void MatchesStore_OnMatchUpdated(Match match)
    {
        this.match = match; 
        ChangeTurns(); 
        UpdateMarks();

        if (IsGameWon())
        {

        }
    }

    private void UpdateMarks()
    {
        for (int i = 0; i < markButtons.Length; i++)
        {
            markButtons[i].UpdateMark(match.marks[i]);
        }
    }
   
    private void ChangeTurns()
    {
        ShowPlayerInsigniaTurn(); 

        foreach(Player player in match.players)
        {
            //  If it's the current user's turn
            if (FirebaseAuth.DefaultInstance.CurrentUser.UserId == match.turn) 
            {
                SetMarkButtonsInteractable(true); 
            }
            else
            {
                SetMarkButtonsInteractable(false); 
            }
        }
    }

    private void SetMarkButtonsInteractable(bool isInteractable)
    {
        foreach(MarkButton markButton in markButtons)
        {
            markButton.gameObject.GetComponent<Button>().interactable = isInteractable; 
        }
    }

    private void SubscribeToMarkButtons()
    {
        for (int i = 0; i < markButtons.Length; i++)
        {
            int index = i;
            markButtons[index].gameObject.GetComponent<Button>().onClick.AddListener(() => MarkButtonOnClick(index)); 
        }
    }

    private void MarkButtonOnClick(int index)
    {
        if (markButtons[index].IsMarked)
            return;

        var updatedMatch = match;
        updatedMatch.marks[index] = GetCurrentPlayer().mark;
        matchesStore.UpdateMatch(updatedMatch); 
    }

    private void ShowPlayerInsigniaTurn()
    {
        foreach(PlayerMarkInsignia insignia in playerMarkInsignias)
        {
            if (insignia.player.id == match.turn)
            {
                insignia.Activate(true); 
            }
            else
            {
                insignia.Activate(false); 
            }
        }
    }

    private Player GetCurrentPlayer()
    {
        foreach(Player player in match.players)
        {
            if (player.id == FirebaseAuth.DefaultInstance.CurrentUser.UserId)
            {
                return player; 
            }
        }

        return null; 
    }

    private bool IsGameWon()
    {
        foreach (Player player in match.players)
        {
            if (match.winner == player.id)
                return true;
        }

        return false;
    }
}
