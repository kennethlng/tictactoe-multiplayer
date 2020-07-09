using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

public class MarkButton : MonoBehaviour
{
    private GameObject xImage;
    private GameObject oImage;
    private Button button;
    private int gridIndex; 
    private bool isMarked = false;
    public bool IsMarked {  get { return isMarked; } }

    #region Events
    public delegate void OnMarkButtonClickedDelegate(int gridIndex);
    public event OnMarkButtonClickedDelegate OnMarkButtonClicked;
    #endregion

    private void Awake()
    {
        xImage = gameObject.transform.Find("X").gameObject;
        oImage = gameObject.transform.Find("O").gameObject;
        button = GetComponent<Button>(); 
    }

    private void Start()
    {
        xImage.SetActive(false);
        oImage.SetActive(false);
        button.interactable = false;
        button.onClick.AddListener(HandleClick);   
    }

    private void OnEnable()
    {
        GameManager.OnTurnChanged += OnGameChangeTurn;
        GameManager.OnMarksUpdated += HandleMarksUpdated;
    }

    private void OnDisable()
    {
        GameManager.OnTurnChanged -= OnGameChangeTurn;
        GameManager.OnMarksUpdated -= HandleMarksUpdated;
    }

    public void Setup(int gridIndex)
    {
        this.gridIndex = gridIndex; 
    }

    private void OnGameChangeTurn(string userId)
    {
        //  If it's the signed-in user's turn, make the button interactable to allow input from the user. 
        button.interactable = userId == FirebaseAuth.DefaultInstance.CurrentUser.UserId;
    }

    private void HandleClick()
    {
        //  If this is already marked, don't do anything
        if (IsMarked)
            return; 

        OnMarkButtonClicked?.Invoke(gridIndex); 
    }

    private void HandleMarksUpdated(List<string> marks)
    {
        string mark = marks[gridIndex];

        if (mark == Constants.MARK_O)
        {
            oImage.SetActive(true);
            xImage.SetActive(false);
            isMarked = true;
        }
        else if (mark == Constants.MARK_X)
        {
            oImage.SetActive(false);
            xImage.SetActive(true);
            isMarked = true;
        }
        else
        {
            oImage.SetActive(false);
            xImage.SetActive(false);
            isMarked = false;
        }
    }
}
