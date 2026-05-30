using System;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using UnityEngine;
using Random = UnityEngine.Random;

public class CitytoMinigameManager : MonoBehaviour
{
    [SerializeField] private List<City> cities = new List<City>();
    [SerializeField, Scene] private List<string>  scenes = new List<string>();
    public List<CityAndMiniGameScene> citiesAndMiniGameScenes = new List<CityAndMiniGameScene>();

    private void Start()
    {
        SetCitiesAndMinigames();
    }

    public void FindCityFromObjectAndLoadMinigame(GameObject minigameObject)
    {
        var city = minigameObject.GetComponent<City>();
        if (city != null)
        {
            var combo = citiesAndMiniGameScenes.FirstOrDefault(c => c.City == city);
            SceneLoaderManager.Instance.LoadMinigameScene(combo.MinigameScene, .5f);
        }
    }
    
    public void SetCitiesAndMinigames()
    {
        
        var sceneList = new List<string>(scenes);
        sceneList.Shuffle();
        foreach (var city in cities)
        {
            var combo = new CityAndMiniGameScene();
            combo.City = city;
            if (sceneList.Count > 0)
            {
                combo.MinigameScene = sceneList[0];
                sceneList.RemoveAt(0);
            }
            else
            {
                combo.MinigameScene = scenes[Random.Range(0, scenes.Count)];
            }
            citiesAndMiniGameScenes.Add(combo);
        }
    }
    
}


[Serializable]
public class CityAndMiniGameScene
{
    public City City;
    [Scene] public string MinigameScene;
}