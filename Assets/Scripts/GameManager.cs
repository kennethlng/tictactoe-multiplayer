using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth; 

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance {  get { return _instance; } }

    public StringVariable matchId; 
    private MatchesStore matchesStore;
    private Match match; 
    public MarkButton[] markButtons;
    public PlayerMarkInsignia[] playerMarkInsignias;
    public Button quitButton;
    public Button playAgainButton; 
    public GameObject gameOverCanvas;
    public Text winnerText;

    #region Events
    public delegate void OnTurnChangedDelegate(string userId);
    public static event OnTurnChangedDelegate OnTurnChanged;
    public delegate void OnMarksUpdatedDelegate(List<string> marks);
    public static event OnMarksUpdatedDelegate OnMarksUpdated; 
    public delegate void OnGameQuitDelegate();
    public static event OnGameQuitDelegate OnGameQuit;
    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this; 
        }

        matchesStore = new MatchesStore(); 
    }

    private void Start()
    {
        gameOverCanvas.SetActive(false);  

        playAgainButton.onClick.AddListener(HandleQuitButtonClick); 
        quitButton.onClick.AddListener(HandleQuitButtonClick);

        SetupMarkButtons();
    }

    private void OnEnable()
    {
        matchesStore.ListenMatch(matchId.Value);
        matchesStore.OnMatchUpdated += MatchesStore_OnMatchUpdated;
    }

    private void OnDisable()
    {
        matchesStore.Unlisten(); 
        matchesStore.OnMatchUpdated -= MatchesStore_OnMatchUpdated;
    }

    private void MatchesStore_OnMatchUpdated(Match match)
    {
        if (match == null) return;

        this.match = match;

        //  If the game is no longer active, don't run any events or game logic
        if (!match.isActive)
        {
            //  Run the game over conditions
            GameOver();
            return;
        }

        SetupPlayerInsignias();

        //  Call events
        OnMarksUpdated?.Invoke(match.marks); 
        OnTurnChanged?.Invoke(match.turn);
    }

    private void SetupMarkButtons()
    {
        for (int i = 0; i < markButtons.Length; i++)
        {
            int index = i;
            markButtons[index].Setup(index); 
            markButtons[index].OnMarkButtonClicked += HandleMarkButtonClick;
        }
    }

    private void HandleMarkButtonClick(int index)
    {
        //  Update the match
        var updatedMatch = match;
        updatedMatch.marks[index] = GetCurrentPlayer().mark;
        matchesStore.UpdateMatch(updatedMatch); 
    }

    private void SetupPlayerInsignias()
    {
        for (int i = 0; i < playerMarkInsignias.Length; i++)
        {
            int index = i;
            playerMarkInsignias[index].Setup(match.players[index]);
        }
    }

    public Player GetCurrentPlayer()
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

    private void HandleQuitButtonClick()
    {
        OnGameQuit?.Invoke(); 
    }

    private void GameOver()
    {
        //  If a winner is declared
        if (string.IsNullOrEmpty(match.winner))
        {
            winnerText.text = "Nobody wins!";
        }
        else
        {
            winnerText.text = "Player " + match.winner + " wins!"; 
        }

        gameOverCanvas.SetActive(true); 
    }
}
