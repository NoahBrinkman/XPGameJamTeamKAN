using System;
using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMainMenuButton : MonoBehaviour
{
    [SerializeField,Scene] private string mainMenuSceneName;

    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (SceneLoaderManager.Instance == null)
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            SceneLoaderManager.Instance.ResetSession();
            SceneLoaderManager.Instance.GoToScene(mainMenuSceneName);
        }
    }
}
