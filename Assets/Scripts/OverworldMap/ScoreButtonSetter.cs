using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreButtonSetter : MonoBehaviour
{
    [SerializeField] private List<Button> easyButtons = new List<Button>();
    [SerializeField] private List<Button> hardButtons = new List<Button>();

    private void Start()
    {
        SceneLoaderManager.Instance?.StartSession();
    }

    private void SetInteractableBasedOnMostRecentScore()
    {
        var scoreStore = SceneLoaderManager.Instance.ScoreStorer;
        if (scoreStore.MostRecentPopcornScore > 3 || scoreStore.CompletedMostRecentClownSays == 1)
        {
            foreach (var button in easyButtons)
            {
                button.interactable = true;
            }

            foreach (var button in hardButtons)
            {
                button.interactable = false;
            }
        }else if (scoreStore.MostRecentTamingScore > 0.75f || scoreStore.MostRecentTinCan == 3)
        {
            foreach (var button in easyButtons)
            {
                button.interactable = false;
            }

            foreach (var button in hardButtons)
            {
                button.interactable = true;
            }
        }
        else
        {
            foreach (var button in easyButtons)
            {
                button.interactable = false;
            }

            foreach (var button in hardButtons)
            {
                button.interactable = false;
            } 
        }
        
    }
    
    private void FixedUpdate()
    {
        SetInteractableBasedOnMostRecentScore();
    }
}
