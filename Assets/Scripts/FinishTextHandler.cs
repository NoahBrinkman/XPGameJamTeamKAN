using System;
using TMPro;
using UnityEngine;

public class FinishTextHandler : MonoBehaviour
{
    [SerializeField]private TMP_Text text;

    private void Start()
    {
        SetTextBasedOnTime();
    }

    public void SetTextBasedOnTime()
    {
        if (SceneLoaderManager.Instance != null)
        {
            TimeSpan t = TimeSpan.FromSeconds(SceneLoaderManager.Instance.SessionTime);
            text.text = string.Format("{0:D2}m:{1:D2}s:{2:D2}ms", t.Minutes, t.Seconds, t.Milliseconds);
        }
    }
}
