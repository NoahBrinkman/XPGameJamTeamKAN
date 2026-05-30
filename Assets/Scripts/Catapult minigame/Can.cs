using System;
using EventBus;
using UnityEngine;

[RequireComponent(typeof(EventBusPublisher))]
public class Can : MonoBehaviour
{
    private bool active = true;
    private void OnCollisionEnter(Collision other)
    {
        if(!active) return;
        
        if (other.transform.CompareTag("Ball"))
        {
            GetComponent<EventBusPublisher>().Publish();
            active = false;
        }
    }
}
