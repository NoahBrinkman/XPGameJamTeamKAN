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
    [SerializeField, Scene] private string endScene;
    [SerializeField] private EventBusPublisher overworldSceneLoadedPublisher;
    [SerializeField] private EventBusPublisher overworldSceneUnLoadedPublisher;
    [SerializeField] private EventBusPublisher startMinigamePublisher;
    [SerializeField] private float delaybeforeStart = 2;
    [SerializeField] private MinigameScoreStorer scoreStorer;
    public MinigameScoreStorer ScoreStorer { get { return scoreStorer; } }
    private float sessionTimer = 0;
    public bool SessionStarted = false;
    [ReadOnly, SerializeField]private int minigamesCompleted = 0;
    [SerializeField] private int minigamesToComplete = 4;
    public float SessionTime {get { return sessionTimer; } }
    [SerializeField] private MusicHandler musicHandler;
    private void Awake()
    {
        InitializeSingleton();
    }

    
    public void ResetSession()
    {
        sessionTimer = 0;
        SessionStarted = false;
        minigamesCompleted = 0;
        minigamesToComplete = 4;
        musicHandler.ResetMusic();
    }
    public void ScoreStoreTinCan(int score)
    {
        scoreStorer.SetTinCanScore(score);
        musicHandler.StopMinigame();
        UnloadMinigameScene();
    }

    private void Update()
    {
        sessionTimer += Time.deltaTime;
    }

    public void StopMinigameMusic()
    {
        musicHandler.StopMinigame();
    }
    public void ScoreStoreClownSays(int score)
    {
        scoreStorer.SetClownSaysScore(score);
    
        UnloadMinigameScene();
    }

    public void ScoreStorePopcorn(int score)
    {
        scoreStorer.SetPopcornScore(score);
        musicHandler.StopMinigame();
        UnloadMinigameScene();
    }

    public void ScoreStoreTame(float score)
    {
        scoreStorer.SetTamingScore(score);
        musicHandler.StopMinigame();
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
        yield return SceneManager.LoadSceneAsync(sceneName, keepCurrentSceneLoaded ? LoadSceneMode.Additive : LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        if (keepCurrentSceneLoaded && sceneName == currentMinigameScene)
        {
            musicHandler.StartMinigameMusic();
        }
        transitionHandler.FadeToTransparent();
        yield return new WaitForSeconds(transitionHandler.Duration);
        
        yield return new WaitForSeconds(delaybeforeStart);
        startMinigamePublisher.Publish();


        if (!keepCurrentSceneLoaded && sceneName == overWorldScene)
        {
            SessionStarted = true;
            musicHandler.FadeInOverworldAgain();
        }
    }

    public void UnloadMinigameScene()
    {
        StartCoroutine(UnloadMinigameSceneAsync());
    }

    private IEnumerator UnloadMinigameSceneAsync()
    {
        transitionHandler.FadeToBlack();
        yield return new WaitForSeconds(transitionHandler.Duration);

        if (minigamesCompleted < minigamesToComplete)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(overWorldScene));
            yield return SceneManager.UnloadSceneAsync(currentMinigameScene);
            overworldSceneLoadedPublisher.Publish();
            
        }
        else
        {
            
            SessionStarted = false;
            yield return SceneManager.LoadSceneAsync(endScene, LoadSceneMode.Single);
        }
        
        
        transitionHandler.FadeToTransparent();
        yield return new WaitForSeconds(transitionHandler.Duration);
    }
    
    public void UnloadScene(string sceneName)
    {
        
    }

    public void StartSession()
    {
        SessionStarted = true;
        musicHandler.FadeInOverworldAgain();
    }

    public void AddToMiniGamesEnded()
    {
        minigamesCompleted++;
    }
}
