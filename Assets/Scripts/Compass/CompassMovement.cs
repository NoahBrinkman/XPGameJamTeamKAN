using System;
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
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _velocity = _rb.linearVelocity;
    }

    public void UpdateAngle(float angle)
    {
        transform.DOLocalRotate(new Vector3(0f, angle, 0f), 0.2f).
            OnComplete(() =>_rb.AddRelativeForce(Vector3.forward * Random.Range(minimumSpeed,maximumSpeed), ForceMode.Force));
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
