using UnityEngine;

public class MinigameScoreStorer : MonoBehaviour
{
    public int CompletedMostRecentClownSays = 0;
    public int MostRecentTinCan = -1;
    public int MostRecentPopcornScore = -1;
    public float MostRecentTamingScore = -1;


    public void SetClownSaysScore(int score)
    {
        CompletedMostRecentClownSays = score;
        MostRecentTinCan = -1;
        MostRecentPopcornScore = -1;
        MostRecentTamingScore = -1;
    }

    public void SetTinCanScore(int score)
    {
        CompletedMostRecentClownSays = -1;
        MostRecentTinCan = score;
        MostRecentPopcornScore = -1;
        MostRecentTamingScore = -1;
    }

    public void SetPopcornScore(int score)
    {
        CompletedMostRecentClownSays = -1;
        MostRecentTinCan = -1;
        MostRecentPopcornScore = score;
        MostRecentTamingScore = -1;
    }

    public void SetTamingScore(float score)
    {
        CompletedMostRecentClownSays = -1;
        MostRecentTinCan = -1;
        MostRecentPopcornScore = -1;
        MostRecentTamingScore = score;
    }

    public void ResetScores()
    {
        CompletedMostRecentClownSays = -1;
        MostRecentTinCan = -1;
        MostRecentPopcornScore = -1;
        MostRecentTamingScore = -1;
    }
}
