using System;
using EventBus;
using UnityEngine;

[RequireComponent(typeof(EventBusPublisher))]
public class Can : MonoBehaviour
{
    private bool active = true;
    [SerializeField] private float knockBackMultiplier = 1.5f;
    
    private void OnCollisionEnter(Collision other)
    {
        if(!active) return;
        
        if (other.transform.CompareTag("Ball"))
        {
            GetComponent<EventBusPublisher>().Publish();
            active = false;
            var direction = transform.position - other.transform.position;
            GetComponent<Rigidbody>().AddForce(direction.normalized * knockBackMultiplier, ForceMode.Impulse);
        }
    }
}
