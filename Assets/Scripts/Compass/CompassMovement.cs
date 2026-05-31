using System;
using System.Collections;
using DG.Tweening;
using EventBus;
using MyBox;
using UnityEngine;
using Random = UnityEngine.Random;

public class CompassMovement : MonoBehaviour
{
    private Sequence _moveSequence;
    private Rigidbody _rb;

    private Vector3 _velocity;

    [SerializeField] private GameObjectPublisher cityHitPublisher;
    [SerializeField] private EventBusPublisher tutorialPublisher;

    [SerializeField] private float minimumSpeed = 175f;
    [SerializeField] private float maximumSpeed = 250f;

    [SerializeField] private AudioSource _driveSound;
    [SerializeField] private AudioSource _yaySound;
    private Sequence drivebuildVolumeDownSequence;
    
    private bool startedMoving = true;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _velocity = _rb.linearVelocity;
        
    }

    private void LateUpdate()
    {
        if (_velocity.magnitude > 0.0001)
        {
            startedMoving = true;
        }
        if (_velocity.magnitude < 0.001f && startedMoving)
        {
            startedMoving = false;
            drivebuildVolumeDownSequence = DOTween.Sequence();
            drivebuildVolumeDownSequence.Append(DOTween.To(() => _driveSound.volume, x => _driveSound.volume = x, 0, 0.2f)).OnComplete(() => _driveSound.Stop());
        }
    }

    
    public void UpdateAngle(float angle)
    {
        transform.DOLocalRotate(new Vector3(0f, angle, 0f), 0.5f).
            OnComplete(() =>
            {
                _rb.AddRelativeForce(Vector3.forward * Random.Range(minimumSpeed, maximumSpeed), ForceMode.Force);
         
            });
        if (drivebuildVolumeDownSequence != null && drivebuildVolumeDownSequence.IsPlaying())
        {
            drivebuildVolumeDownSequence.Kill();
            
        }
        _driveSound.volume = 1;
        _driveSound.Play();
    }

    public void AllowMovement(bool canMove)
    {
        if (canMove)
        {
            _rb.isKinematic = false;
            _rb.useGravity = true;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Bounds"))
        {
            float speed = _velocity.magnitude;
            Vector3 dir = Vector3.Reflect(_velocity.normalized, col.contacts[0].normal);
            _rb.linearVelocity = dir * speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("City"))
        {
            Debug.Log("CITY!");
            _driveSound.Stop();
            _yaySound.Play();
            _rb.isKinematic = true;
            _rb.useGravity = false;
            transform.position = other.gameObject.transform.position;
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            var city = other.GetComponent<City>();
            if (city != null)
            {
                city.MeshSwap();
            }
            cityHitPublisher?.SetValueAndPublish(other.gameObject);
        }

        if (other.gameObject.CompareTag("Tutorial"))
        {
            tutorialPublisher.Publish();
        }
    }
}
