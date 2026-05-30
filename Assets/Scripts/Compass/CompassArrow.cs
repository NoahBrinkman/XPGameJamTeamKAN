using System;
using DG.Tweening;
using EventBus;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Compass : MonoBehaviour
{
    [Header("Spin")] 
    [SerializeField] [Range(2f, 5f)] private float spinDuration = 2.5f;
    [SerializeField] private int minSpins = 5;
    [SerializeField] private int maxSpins = 8;

    [Header("Pendulum")] 
    [SerializeField] [Range(0.1f, 1f)] private float pendulumDuration = 0.5f;
    [SerializeField] private int pendulumCycles = 5;
    [SerializeField] private float pendulumWobbleDur = 2.0f;
    [SerializeField] public float pendulumMinAngle = 90f;
    [SerializeField] public float pendulumMaxAngle = 270f;

    [Header("Wobble")] 
    [SerializeField] private float wobbleStrength = 8f;
    [SerializeField] private int wobbleVibrato = 10;
    [SerializeField] private float wobbleDuration = 0.5f;
    
    [Header("Power-ups")]
    public bool powerActive;

    private Sequence _spinSequence;
    private Sequence _pendulumSequence;
    private FloatPublisher _newAngle;
    

    private void Awake()
    {
        _newAngle = GetComponent<FloatPublisher>();
    }

    public void SetDirection(DirectionsData data)
    {
        powerActive = true;
        pendulumMinAngle = data.minAngle;
        pendulumMaxAngle = data.maxAngle;
    }

    public void Spin()
    {
        if(powerActive)
        {   
            Debug.Log("Pendulum");
            Pendulum();
        }
        else
        {
            FullSpin();
        }
    }

    private void Pendulum()
    {
        if (_pendulumSequence != null && _pendulumSequence.IsActive())
            _pendulumSequence.Kill();

        float finalAngle = Random.Range(pendulumMinAngle, pendulumMaxAngle);
        Debug.Log($"Final angle pendulum: {finalAngle}");

        float finalSpeed = Mathf.Max(
            0.05f,
            (finalAngle - pendulumMinAngle) * pendulumDuration /
            (pendulumMaxAngle - pendulumMinAngle)
        );

        _pendulumSequence = DOTween.Sequence();

        transform.localRotation = Quaternion.Euler(0f, 0f, pendulumMinAngle);

        float sweep = pendulumMaxAngle - pendulumMinAngle;
        
        for (int i = 0; i < pendulumCycles; i++)
        {
            _pendulumSequence.Append(
                transform.DOLocalRotate(
                    new Vector3(0f, 0f, sweep),
                    pendulumDuration,
                    RotateMode.LocalAxisAdd
                ).SetEase(Ease.Linear)
            );

            _pendulumSequence.Append(
                transform.DOLocalRotate(
                    new Vector3(0f, 0f, -sweep),
                    pendulumDuration,
                    RotateMode.LocalAxisAdd
                ).SetEase(Ease.Linear)
            );
        }
        _pendulumSequence.Append(
            transform.DOLocalRotate(
                new Vector3(0f, 0f, pendulumMinAngle),
                0.01f
            )
        );
        _pendulumSequence.Append(
            transform.DOLocalRotate(
                new Vector3(0f, 0f, finalAngle),
                finalSpeed
            ).SetEase(Ease.OutQuad)
        );
        
        _pendulumSequence.Append(
            transform.DOPunchRotation(
                new Vector3(0f, 0f, wobbleStrength),
                pendulumWobbleDur,
                20,
                0.8f
            )
        );

        _pendulumSequence.OnComplete(PublishAngle);

        _pendulumSequence.Play();
        powerActive = false;
    }

    private void FullSpin()
    {
        if (_spinSequence != null && _spinSequence.IsActive())
            _spinSequence.Kill();

        float finalAngle = Random.Range(0f, 360f);
        int spins = Random.Range(minSpins, maxSpins + 1);

        float totalAngle = (360f * spins) + finalAngle;

        _spinSequence = DOTween.Sequence();

        _spinSequence.Append(transform.DOLocalRotate(
            new Vector3(0f, 0f, totalAngle),
            spinDuration,
            RotateMode.FastBeyond360
        ).SetEase(Ease.OutCubic)).
            OnComplete(PublishAngle);

        _spinSequence.Insert(0.7f * spinDuration, transform.DOPunchRotation(
            new Vector3(0f, 0f, wobbleStrength),
            wobbleDuration,
            wobbleVibrato,
            0.8f
        ));
        _spinSequence.Play();
    }

    private void PublishAngle()
    {
        _newAngle.SetValue(transform.localEulerAngles.z);
        _newAngle.Publish();
    }
}