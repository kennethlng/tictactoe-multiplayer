using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float rotateSpeed = 200f;
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>(); 
    }

    void Update()
    {
        rect.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);         
    }
}
