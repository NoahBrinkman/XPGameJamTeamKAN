using System.Collections;
using UnityEngine;

public class MinigameUnloader : MonoBehaviour
{
    [SerializeField] private float delay = 1;
    private bool ended = false;
    public void EndPopcornGame(int score)
    {
        if(ended) return;
        ended = true;
        if (SceneLoaderManager.Instance != null)
        {
            SceneLoaderManager.Instance.AddToMiniGamesEnded();
            SceneLoaderManager.Instance?.StopMinigameMusic();
             StartCoroutine(EndPopcorn(score));
        }
    }

    IEnumerator EndPopcorn(int score)
    {
        yield return new WaitForSeconds(delay);
        SceneLoaderManager.Instance.ScoreStorePopcorn(score);
    }
    public void EndClownSaysGame(int score)
    {
         if(ended) return;
        ended = true;
        if (SceneLoaderManager.Instance != null)
        {
            SceneLoaderManager.Instance.AddToMiniGamesEnded();
            SceneLoaderManager.Instance?.StopMinigameMusic();
            StartCoroutine(EndSays(score));
        }
    }

    IEnumerator EndSays(int score)
    {
        yield return new WaitForSeconds(delay);
        SceneLoaderManager.Instance?.ScoreStoreClownSays(score);
    }
    public void EndTinCanGame(int score)
    {
        if(ended) return;
        ended = true;
        if (SceneLoaderManager.Instance != null)
        {
            SceneLoaderManager.Instance.AddToMiniGamesEnded();
            SceneLoaderManager.Instance?.StopMinigameMusic();
            StartCoroutine(EndTin(score));
        }
    }

    IEnumerator EndTin(int score)
    {
        yield return new WaitForSeconds(delay);
        SceneLoaderManager.Instance?.ScoreStoreTinCan(score);
    }
    public void EndTameGame(float score)
    {
        if(ended) return;
        ended = true;
        if (SceneLoaderManager.Instance != null)
        {
            
            SceneLoaderManager.Instance.AddToMiniGamesEnded();
            SceneLoaderManager.Instance?.StopMinigameMusic();
            StartCoroutine(EndTame(score));
        }
    }

    IEnumerator EndTame(float score)
    {
        yield return new WaitForSeconds(delay);
        SceneLoaderManager.Instance?.ScoreStoreTame(score);
    }
    
}
