using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public delegate void OnMatchFindDelegate();
    public static event OnMatchFindDelegate OnMatchFind; 
    public delegate void OnMatchSelectedDelegate();
    public static event OnMatchSelectedDelegate OnMatchSelected; 
    private MatchesStore matchesStore;
    private QueuesStore queuesStore; 
    public StringVariable matchId;
    public Button findMatchButton;
    public RectTransform listContent;
    public GameObject listItemPrefab; 

    private void Awake()
    {
        queuesStore = new QueuesStore();
        matchesStore = new MatchesStore(); 
    }

    private void OnEnable()
    {
        MatchesStore.OnMatchesUpdated += OnMatchesUpdated;
        matchesStore.ListenMatches();
    }

    private void OnDisable()
    {
        MatchesStore.OnMatchesUpdated -= OnMatchesUpdated;
        matchesStore.Unlisten(); 
    }

    void Start()
    {
        findMatchButton.interactable = true;
        findMatchButton.onClick.AddListener(FindMatchButtonClicked);
    }

    private void FindMatchButtonClicked()
    {
        Debug.Log("Find Match button clicked");

        //  Disable the button to prevent multiple queues from being created
        findMatchButton.interactable = false;

        queuesStore.CreateQueue();
    }

    private void OnMatchesUpdated(List<Match> matches)
    {
        //  First remove the current list items
        foreach(Transform child in listContent.transform)
        {
            Destroy(child.gameObject); 
        }

        //  Add a list item for each match found
        for (int i = 0; i < matches.Count; i++)
        {
            var index = i; 
            GameObject listItemGameObject = Instantiate(listItemPrefab, listContent, false); 
            ListItem listItem = listItemGameObject.GetComponent<ListItem>();
            listItem.title.text = matches[i].id;
            listItem.button.onClick.AddListener(() => SelectMatch(matches[index])); 
        }
    }

    private void SelectMatch(Match match)
    {
        //  Save the match ID in the scriptableObject so that the GameManager can access it in the GameScene
        matchId.Value = match.id;

        //  Broadcast an event so the StateManager can tell the SceneController to load the GameScene
        OnMatchSelected?.Invoke(); 
    }
}
