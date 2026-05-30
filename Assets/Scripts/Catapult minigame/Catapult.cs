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

    private float timer = 0.0f;
        
    [SerializeField] private bool isSwinging = false;

    [SerializeField] private int shots = 3;
    private int shotsLeft = 0;

    private IntPublisher shotPublisher;

    private void OnEnable()
    {
        shotPublisher = GetComponent<IntPublisher>();
        
    }

    [ButtonMethod]
    public void StartMiniGame()
    {
        shotsLeft = shots;
        timer = 0.0f;
        isSwinging = true;
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
        if (isSwinging)
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
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isSwinging)
        {
            timer += Time.deltaTime;
            float t = 0.5f + Mathf.Sin(timer * swingSpeed)/ 2;
            Vector3 eulerAngles = transform.localEulerAngles;
            eulerAngles.y = Mathf.Lerp(-(swingRadius/2), swingRadius/2, t);
            transform.localEulerAngles = eulerAngles;
        }
    }

    private void OnDrawGizmos()
    {
        var direction = transform.forward;
        direction.y += launchHeightMultiplier;
        
        Debug.DrawRay(transform.position, direction * launchSpeed, Color.red);
    }
}
