using System;
using DG.Tweening;
using MyBox;
using UnityEngine;

public class CompassMovement : MonoBehaviour
{
    private Sequence _moveSequence;
    private Rigidbody _rb;

    private Vector3 _velocity;

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
            OnComplete(() =>_rb.AddRelativeForce(Vector3.forward * 200f, ForceMode.Force));
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
}
