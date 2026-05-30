using System;
using EventBus;
using UnityEngine;
using Random = UnityEngine.Random;

public class Popcorn : MonoBehaviour
{
    [SerializeField] private float speed = 1.2f;
    private Rigidbody rb;
    private bool isPopped = false;
    [SerializeField] private GameObject unpoppedKernelMesh;
    [SerializeField] private GameObject poppedKernelMesh;
    [SerializeField] private GameObject popcornFluff;
    private EventBusPublisher poppedEvent;
   private void OnEnable()
    {
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = direction * speed;
        poppedEvent = GetComponent<EventBusPublisher>();
    }

    private void Update()
    {
        if (!isPopped)
        {
            Vector3 vel = rb.linearVelocity;
            Vector3.ClampMagnitude(vel, speed);
            rb.linearVelocity = vel;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(isPopped) return;
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        rb.linearVelocity = direction * speed;
    }

    public void Pop()
    {
        if(isPopped) return;
        rb.useGravity = true;
        unpoppedKernelMesh.SetActive(false);
        poppedKernelMesh.SetActive(true);
        isPopped = true;
        popcornFluff.SetActive(true);
        rb.constraints = RigidbodyConstraints.None;
        if(poppedEvent != null) poppedEvent.Publish();
    }
}
