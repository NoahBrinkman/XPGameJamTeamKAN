using System.Collections.Generic;
using EventBus;
using UnityEngine;

public class PopcornStarter : MonoBehaviour
{
    [SerializeField]List<GameObject> popcorns = new List<GameObject>();
    [SerializeField] private int minimum = 4;
    [SerializeField] private int maximum = 7;

    public void EnableRandom()
    {
        int amount = Random.Range(minimum, maximum + 1);
        for (int i = 0; i < amount; i++)
        {
            popcorns[i].SetActive(true);
        }
        GetComponent<IntPublisher>().SetValueAndPublish(amount);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
