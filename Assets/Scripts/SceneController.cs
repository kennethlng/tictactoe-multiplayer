using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneController : MonoBehaviour
{
    public string initialSceneName;
    public float fadeDuration = 1.0f;
    private bool isFading = false;
    public CanvasGroup faderCanvasGroup;
    public delegate void BeforeSceneUnloadDelegate();
    public static event BeforeSceneUnloadDelegate OnBeforeSceneUnload;
    public delegate void AfterSceneLoadDelegate();
    public static event AfterSceneLoadDelegate OnAfterSceneLoad;

    private IEnumerator Start()
    {
        faderCanvasGroup.alpha = 1f;

        yield return StartCoroutine(LoadSceneAndSetActive(initialSceneName));

        //StartCoroutine(Fade(0f)); 
    }

    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

        SceneManager.SetActiveScene(newlyLoadedScene);
    }

    public void FadeAndLoadScene(string sceneName)
    {
        if (!isFading)
            StartCoroutine(FadeAndSwitchScenes(sceneName)); 
    }

    private IEnumerator FadeAndSwitchScenes(string sceneName)
    {
        yield return StartCoroutine(Fade(1f));

        OnBeforeSceneUnload?.Invoke();

        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        OnAfterSceneLoad?.Invoke();

        yield return StartCoroutine(Fade(0f)); 
    }

    private IEnumerator Fade(float finalAlpha)
    {
        isFading = true;

        faderCanvasGroup.blocksRaycasts = true;

        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;

        while(!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
        {
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        isFading = false;

        faderCanvasGroup.blocksRaycasts = false; 
    }
}
