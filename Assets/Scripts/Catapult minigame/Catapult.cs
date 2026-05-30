using System;
using EventBus;
using MyBox;
using UnityEngine;
using UnityEngine.InputSystem;

public class Catapult : MonoBehaviour
{
    [SerializeField] private InputAction _inputAction;
    
    [SerializeField] private float swingSpeed = 1;

    [SerializeField] private GameObject ballPrefab;
    
    [SerializeField] private float swingRadius = 160;

    [SerializeField] private float launchSpeed = 15;
    [SerializeField] private float launchHeightMultiplier = 1.5f;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private float gameDuration = 10;
    private bool gameTimerActive = false;
    private float gameTimer;

    [SerializeField] private float timeAfterLastShot = 2;
    private bool lastShotTimerActive = false;
    private float lastShotTimer = 0;
    
    private float swingTimer = 0.0f;
        
    [SerializeField] private bool isSwinging = false;

    [SerializeField] private int shots = 3;
    private int shotsLeft = 0;

    private IntPublisher shotPublisher;
    
    [SerializeField] private IntPublisher gameOverPublisher;
    private int score = 0;
    
    private void OnEnable()
    {
        shotPublisher = GetComponent<IntPublisher>();
        
    }

    public void AddToScore()
    {
        score++;
    }
    
    [ButtonMethod]
    public void StartMiniGame()
    {
        gameTimer = gameDuration;
        shotsLeft = shots;
        swingTimer = 0.0f;
        lastShotTimer = timeAfterLastShot;
        isSwinging = true;
        gameTimerActive = true;
        if (shotPublisher != null)
        {
            shotPublisher.SetValue(shotsLeft);
            shotPublisher.Publish();
        }
        _inputAction.Enable();
          _inputAction.performed += Shoot;
    }

    private void Shoot(InputAction.CallbackContext obj)
    {
        if (isSwinging && shotsLeft > 0)
        {
            shotsLeft--;
            if (shotPublisher != null)
            {
                shotPublisher.SetValue(shotsLeft);
                shotPublisher.Publish();
            }

            var ball = Instantiate(ballPrefab);
            Vector3 startpos = transform.position;
            startpos.y += 0.5f;
            ballPrefab.transform.position = startpos;
            var direction = transform.forward;
            direction.y += launchHeightMultiplier;
            ball.GetComponent<Rigidbody>().AddForce(direction.normalized * launchSpeed, ForceMode.Impulse);
            if(sfxSource != null) sfxSource.Play();
            if (shotsLeft == 0 && gameTimer > lastShotTimer)
            {
                lastShotTimerActive = true;
                lastShotTimer = timeAfterLastShot;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isSwinging)
        {
            swingTimer += Time.deltaTime;
            float t = 0.5f + Mathf.Sin(swingTimer * swingSpeed)/ 2;
            Vector3 eulerAngles = transform.localEulerAngles;
            eulerAngles.y = Mathf.Lerp(-(swingRadius/2), swingRadius/2, t);
            transform.localEulerAngles = eulerAngles;
            if (gameTimerActive)
                gameTimer -= Time.deltaTime;
            if(lastShotTimerActive)
                lastShotTimer -= Time.deltaTime;

            if (gameTimer <= 0 || lastShotTimer <= 0)
            {
                EndGame();
            }

        }
    }

    public void EndGame()
    {
        isSwinging = false;
        gameTimerActive = false;
        lastShotTimerActive = false;
        gameOverPublisher.SetValueAndPublish(score);
        Debug.Log($"Tin can mini game ended with score {score}");
    }
    
    private void OnDrawGizmos()
    {
        var direction = transform.forward;
        direction.y += launchHeightMultiplier;
        
        Debug.DrawRay(transform.position, direction * launchSpeed, Color.red);
    }
}
