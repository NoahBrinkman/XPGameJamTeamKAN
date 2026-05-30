using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EventBus;
using MyBox;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SimonSays : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private InputAction click;
    [SerializeField] private Image[] lights;
    [SerializeField] private List<Transform> clowns;
    [SerializeField] private List<Transform> positions;
    [SerializeField] private Color[] colors;

    private readonly List<int> _correctSequence = new();
    private int _currentTry;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private IntPublisher _tries;
    [SerializeField] private EventBusPublisher _correct;
    [SerializeField] private EventBusPublisher _incorrect;
    [SerializeField] private IntPublisher _finish;
    private int _try = 3;
    private List<Transform> _clownsToUse = new();
    [SerializeField] private List<AudioSource> beepsources = new();
    
    
    public static void Shuffle<T>(IList<T> ts) {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = UnityEngine.Random.Range(i, count);
            (ts[i], ts[r]) = (ts[r], ts[i]);
        }
    }

    private void OnEnable()
    {
        click.Enable();
        click.performed += ClickOnPerformed;
        _correctSequence.Clear();
        _clownsToUse.AddRange(clowns);
    }

    private void Start()
    {
        _tries = GetComponent<IntPublisher>();
        _try = 3;
        Shuffle(_clownsToUse);
        for (int i = 0; i < 4; i++)
        {
            _clownsToUse[i].transform.position = positions[i].transform.position;
            _clownsToUse[i].transform.rotation = positions[i].transform.rotation;
        }
        
        foreach (var clown in clowns)
        {
            clown.GetComponent<BoxCollider>().enabled = false;
        }
        
        StartCoroutine(ExampleCoroutine());
    }

    IEnumerator ExampleCoroutine()
    {
        for (int i = 0; i < 4; i++)
        {
            int rand = Random.Range(0, 4);
            lights[i].color = colors[rand];
            _correctSequence.Add(rand);
            //BEEP
            beepsources[i].Play();
            yield return new WaitForSeconds(0.8f);
        }
        //BEEEEEEP
        beepsources[beepsources.Count - 1].Play();
        yield return new WaitForSeconds(0.3f);
        foreach (var light in lights)
        {
            light.color = Color.white;
        }

        foreach (var clown in clowns)
        {
            clown.GetComponent<BoxCollider>().enabled = true;
        }

        foreach (var number in _correctSequence)
        {
            Debug.Log(number);
        }
    }

    private void ClickOnPerformed(InputAction.CallbackContext obj)
    {
        var mousepos= Mouse.current.position.ReadValue();
        var ray = cam.ScreenPointToRay(new Vector3(mousepos.x, mousepos.y, cam.transform.position.z));
        if (Physics.Raycast(ray, out var hit, 1000f, layerMask))
        {
            int clownId = clowns.IndexOf(hit.transform);
            Debug.Log(clownId);

            if (_correctSequence[_currentTry] == clownId)
            {
                Debug.Log("CORRECT");
                _correct.Publish();
                var currentPosition = clowns[clownId].transform.position;
                clowns[clownId].transform.DOJump(currentPosition, 0.6f, 1, 0.15f);
                _currentTry++;
            }
            else
            {
                Debug.Log("INCORRECT");
                _incorrect.Publish();
                foreach (var light in lights)
                {
                    light.transform.DOShakePosition(0.6f, new Vector3(4f, 0 , 4f));
                }
                _currentTry = 0;
                _try--;
                if (_try <= 0)
                {
                    _finish.SetValue(0);
                    SceneLoaderManager.Instance.ScoreStorer.SetClownSaysScore(0);
                    StartCoroutine(WaitAndFinish());
                }
                Debug.Log(_try);
                _tries.SetValueAndPublish(_try);
            }

            if (_currentTry >= _correctSequence.Count)
            {
                Debug.Log("DONE");
                _finish.SetValueAndPublish(1);
                
                SceneLoaderManager.Instance.ScoreStorer.SetClownSaysScore(1);
                foreach (var clown in clowns)
                {
                    var currentPosition = clown.transform.position;
                    clown.transform.DOJump(currentPosition, 0.6f, 1, 0.2f);
                    var finalRotation = clown.transform.rotation.y + 540;
                    clown.transform.DORotate(new Vector3(0, finalRotation, 0), 0.5f, RotateMode.FastBeyond360);
                }

                StartCoroutine(WaitAndFinish());
                _currentTry = 0;
            }
        }
        
    }

    IEnumerator WaitAndFinish()
    {
        yield return new WaitForSeconds(0.3f);
        _finish.Publish();
        SceneLoaderManager.Instance.UnloadMinigameScene();
    }

}
