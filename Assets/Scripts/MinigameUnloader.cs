using System.Collections;
using UnityEngine;

public class MinigameUnloader : MonoBehaviour
{
    [SerializeField] private float delay = 1;

    public void EndPopcornGame(int score)
    {
        if (SceneLoaderManager.Instance != null)
        {
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
        if (SceneLoaderManager.Instance != null)
        {
            StartCoroutine(EndSays(score));
        }
    }

    IEnumerator EndSays(int score)
    {
        yield return new WaitForSeconds(delay);
        SceneLoaderManager.Instance.ScoreStoreClownSays(score);
    }
    public void EndTinCanGame(int score)
    {
        if (SceneLoaderManager.Instance != null)
        {
            StartCoroutine(EndTin(score));
        }
    }

    IEnumerator EndTin(int score)
    {
        yield return new WaitForSeconds(delay);
        SceneLoaderManager.Instance.ScoreStoreTinCan(score);
    }
    public void EndTameGame(float score)
    {
        if (SceneLoaderManager.Instance != null)
        {
            StartCoroutine(EndTame(score));
        }
    }

    IEnumerator EndTame(float score)
    {
        yield return new WaitForSeconds(delay);
        SceneLoaderManager.Instance.ScoreStoreTame(score);
    }
    
}
