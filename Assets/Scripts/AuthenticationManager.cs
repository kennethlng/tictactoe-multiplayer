using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth; 

public class AuthenticationManager : MonoBehaviour
{
    private static AuthenticationManager _instance;
    public static AuthenticationManager Instance {  get { return _instance; } }

    private FirebaseAuth auth;
    public static FirebaseUser user = null;

    public delegate void SignOutDelegate();
    public delegate void SignInDelegate();
    public delegate void SigningInDelegate();
    public delegate void SignInFailedDelegate();
    public static event SignOutDelegate OnSignedOut;
    public static event SignInDelegate OnSignedIn;
    public static event SigningInDelegate OnSigningIn;
    public static event SignInFailedDelegate OnSignInFailed;

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

    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += OnAuthStateChanged;
        //OnAuthStateChanged(this, null);
    }

    private void OnAuthStateChanged(object sender, System.EventArgs e)
    {
        if (auth.CurrentUser != user)
        {
            bool isSignedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!isSignedIn && user != null)
            {
                Debug.Log("Signed out: " + user.UserId);

                OnSignedOut?.Invoke();
            }

            user = auth.CurrentUser;

            if (isSignedIn)
            {
                Debug.Log("Signed in " + user.UserId);

                OnSignedIn?.Invoke();
            }
        }
    }

    public Task SignInAnonymously()
    {
        return auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                OnSignInFailed?.Invoke();  
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                OnSignInFailed?.Invoke();  
                return;
            }

            user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);
        });
    }

    public void SignOut()
    {
        auth.SignOut(); 
    }
}
