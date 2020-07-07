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
    public StringVariable matchId; 
    private MatchesStore matchesStore;
    private Match match; 
    public MarkButton[] markButtons;
    public PlayerMarkInsignia[] playerMarkInsignias;

    private void Awake()
    {
        matchesStore = new MatchesStore(); 
    }

    private void Start()
    {
        matchesStore.ListenMatch(matchId.Value);
        matchesStore.OnMatchUpdated += MatchesStore_OnMatchUpdated;

        SubscribeToMarkButtons();

        Debug.Log(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
    }

    private void MatchesStore_OnMatchUpdated(Match match)
    {
        this.match = match; 

        //  If a winner is declared
        if (!string.IsNullOrEmpty(match.winner))
        {
            return; 
        }

        ChangeTurns();
        UpdateMarks();
    }

    private void UpdateMarks()
    {
        for (int i = 0; i < markButtons.Length; i++)
            markButtons[i].UpdateMark(match.marks[i]);
        
    }
   
    private void ChangeTurns()
    {
        ShowPlayerInsigniaTurn();

        if (match.turn == FirebaseAuth.DefaultInstance.CurrentUser.UserId)
        {
            SetMarkButtonsInteractable(true); 
            return;
        }

        SetMarkButtonsInteractable(false); 
    }

    private void SetMarkButtonsInteractable(bool isInteractable)
    {
        foreach(MarkButton markButton in markButtons)
            markButton.gameObject.GetComponent<Button>().interactable = isInteractable; 
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

        //  Once the player has inputted an action, disable all the buttons to prevent unnecessary repeated actions
        SetMarkButtonsInteractable(false);

        //  Update the match
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
}
