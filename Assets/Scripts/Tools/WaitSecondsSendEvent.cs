using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WaitSecondsSendEvent : MonoBehaviour
{
    [SerializeField] private float waitTime = 1;
    [SerializeField] private UnityEvent onFinished;
    [SerializeField] private bool runOnEnable = false;

    private void OnEnable()
    {
        if(runOnEnable) WaitAndSend();
    }

    public void WaitAndSend()
    {
        StartCoroutine(WaitForSecondsSendEvent());
    }

    private IEnumerator WaitForSecondsSendEvent()
    {
        yield return new WaitForSeconds(waitTime);
        onFinished?.Invoke();
        
    }
}
