using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{

    public FadeScreen fadeScreen;
    public static SceneTransitionManager Instance;
    private bool isLoadingScene = true;


    [Header("Events")]
    public UnityEvent OnSceneLoadStarted;
    public UnityEvent OnSceneLoadEnded;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }

    private void OnEnable()
    {
        OnSceneLoadStarted.AddListener(SceneLoadStarted);
        OnSceneLoadEnded.AddListener(SceneLoadEnded);
    }


    private void OnDisable()
    {
        OnSceneLoadStarted.RemoveListener(SceneLoadStarted);
        OnSceneLoadEnded.RemoveListener(SceneLoadEnded);
    }
    private void SceneLoadEnded()
    {
        isLoadingScene = false;
    }
    private void SceneLoadStarted()
    {
        isLoadingScene = true;
    }


    [ContextMenu(nameof(SetComponents))]
    public void SetComponents()
    {
        fadeScreen = FindObjectOfType<FadeScreen>();
    }

    public void LoadScene(SceneField sceneIndex)
    {
        if (isLoadingScene) return;
        StartCoroutine(LoadSceneRoutine(sceneIndex));
    }

    IEnumerator LoadSceneRoutine(SceneField sceneIndex)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(fadeScreen.fadeDuration);

        //Launch the new scene
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadSceneAsync(SceneField sceneIndex)
    {
        if (isLoadingScene) return;
        StartCoroutine(LoadSceneAsyncRoutine(sceneIndex));
    }

    IEnumerator LoadSceneAsyncRoutine(SceneField sceneIndex)
    {
        fadeScreen.FadeOut();
        //Launch the new scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        float timer = 0;
        while (timer <= fadeScreen.fadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        operation.allowSceneActivation = true;
    }
}
