using System;
using System.Collections;
using EventBus;
using MyBox;
using UnityEngine;
using UnityEngine.InputSystem;

public class PopcornClicker : MonoBehaviour
{
    [SerializeField] private InputAction shoot;
    [ SerializeField] private LayerMask layerMask;
   
    [SerializeField] private Camera cam;
    
    private bool gameStarted = false;
    [SerializeField] private float gameDuration = 10;
    private float gameTimer;
    [SerializeField] private IntPublisher gameOverPublisher;
    private int score = 0;

    [SerializeField] private float earlyEndTime = 1;
    private int maxAmount = 5;
    private float earlyEndTimer = 1;
    private bool startEarlyEnd = false;
    private void OnEnable()
    {
        shoot.Enable();
        gameTimer = gameDuration;
        shoot.performed += ShootOnperformed;
        score = 0;
    }

    [ButtonMethod]
    public void StartGame()
    {
        gameStarted = true;
        gameTimer = gameDuration;
        score = 0;
        earlyEndTimer = earlyEndTime;
    }
    
    private void ShootOnperformed(InputAction.CallbackContext obj)
    {
        if(!gameStarted)
            return;
        
       var mousepos= Mouse.current.position.ReadValue();
        var ray = cam.ScreenPointToRay(new Vector3(mousepos.x, mousepos.y, cam.transform.position.z));
        if (Physics.Raycast(ray, out var hit, 1000f, layerMask))
        {
            var popCorn = hit.transform.GetComponent<Popcorn>();
            popCorn.Pop();
        }
    }

    private void Update()
    {
        if(!gameStarted) 
            return;
        
        gameTimer -= Time.deltaTime;
        if (startEarlyEnd)
        {
            earlyEndTimer -= Time.deltaTime;
        }
        if (gameTimer <= 0 || earlyEndTimer <= 0)
        {
           gameStarted = false;
           gameOverPublisher.SetValueAndPublish(score);
        }
    }

    public void AddScore()
    {
        score++;
        if (score >= maxAmount && gameTimer > earlyEndTime)
        {
            startEarlyEnd = true;
        }
    }

    public void SetMaxAmount(int amount)
    {
        maxAmount = amount;
    }
    
}
