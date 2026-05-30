using System;
using System.Collections;
using EventBus;
using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : Singleton<SceneLoaderManager>
{
    private string currentMinigameScene;
    [SerializeField] private TransitionHandler transitionHandler;
    [SerializeField, Scene] private string overWorldScene;
    [SerializeField] private EventBusPublisher overworldSceneLoadedPublisher;
    [SerializeField] private EventBusPublisher overworldSceneUnLoadedPublisher;
    [SerializeField] private EventBusPublisher startMinigamePublisher;
    [SerializeField] private float delaybeforeStart = 2;
    [SerializeField] private MinigameScoreStorer scoreStorer;
    private void Awake()
    {
        InitializeSingleton();
    }


    public void ScoreStoreTinCan(int score)
    {
        scoreStorer.SetTinCanScore(score);
        UnloadMinigameScene();
    }
    
    public void ScoreStoreClownSays(int score)
    {
        scoreStorer.SetClownSaysScore(score);
        UnloadMinigameScene();
    }

    public void ScoreStorePopcorn(int score)
    {
        scoreStorer.SetPopcornScore(score);
        UnloadMinigameScene();
    }

    public void ScoreStoreTame(float score)
    {
        scoreStorer.SetTamingScore(score);
        UnloadMinigameScene();
    }

    public void GoToScene(string sceneName)
    {
        StartCoroutine(LoadSceneInAsync(sceneName, false));
    }
    
    public void LoadMinigameScene(string minigameScene, float delay = 0)
    {
        currentMinigameScene = minigameScene;
        StartCoroutine(LoadSceneInAsync(currentMinigameScene));
        
    }
    
    public void LoadSceneAsync(string sceneName, bool keepCurrentSceneLoaded = true)
    {
                
    }

    private IEnumerator LoadSceneInAsync(string sceneName, bool keepCurrentSceneLoaded = true, float delay = 0)
    {
        if (delay != 0)
        {
            yield return new WaitForSeconds(delay);
        }
        transitionHandler.FadeToBlack();
        yield return new WaitForSeconds(transitionHandler.Duration);
        if (SceneManager.GetActiveScene().name == overWorldScene)
        {
            overworldSceneUnLoadedPublisher.Publish();
        }
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        
        if (!keepCurrentSceneLoaded)
        {
            if (currentMinigameScene != "")
            {
                yield return SceneManager.UnloadSceneAsync(currentMinigameScene);
            }
        }   
        transitionHandler.FadeToTransparent();
        yield return new WaitForSeconds(transitionHandler.Duration);
        
        yield return new WaitForSeconds(delaybeforeStart);
        startMinigamePublisher.Publish();
    }

    public void UnloadMinigameScene()
    {
        StartCoroutine(UnloadMinigameSceneAsync());
    }

    private IEnumerator UnloadMinigameSceneAsync()
    {
        transitionHandler.FadeToBlack();
        yield return new WaitForSeconds(transitionHandler.Duration);
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(overWorldScene));
        yield return SceneManager.UnloadSceneAsync(currentMinigameScene);
        overworldSceneLoadedPublisher.Publish();
        transitionHandler.FadeToTransparent();
        yield return new WaitForSeconds(transitionHandler.Duration);
        
    }
    
    public void UnloadScene(string sceneName)
    {
        
    }
    
}
