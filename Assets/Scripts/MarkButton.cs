using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkButton : MonoBehaviour
{
    private GameObject xImage;
    private GameObject oImage;
    private bool isMarked = false;
    public bool IsMarked {  get { return isMarked; } }

    private void Awake()
    {
        xImage = gameObject.transform.Find("X").gameObject;
        oImage = gameObject.transform.Find("O").gameObject;
    }

    private void Start()
    {
        xImage.SetActive(false);
        oImage.SetActive(false); 
    }

    public void UpdateMark(string mark)
    {
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
