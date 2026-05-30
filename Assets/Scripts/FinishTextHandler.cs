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
            text.text =
                $"You finished with the following time: {TimeSpan.FromSeconds(SceneLoaderManager.Instance.SessionTime)}";
        }
    }
}
