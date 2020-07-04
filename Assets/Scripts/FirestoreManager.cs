using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions; 

public class FirestoreManager : MonoBehaviour
{
    private FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

    private static FirestoreManager _instance;
    public static FirestoreManager Instance {  get { return _instance; } }

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
    }
}
