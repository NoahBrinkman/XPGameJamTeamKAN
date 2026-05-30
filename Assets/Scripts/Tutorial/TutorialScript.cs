using System;
using DG.Tweening;
using EventBus;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    private float _spinDuration = 2.5f;
    private int _spins = 10;
    private float _spinAngle = 140f;
    private float _pendulumDuration = 0.14f;
    private int _pendulumCycles = 5;
    private float _wobbleDuration = 0.2f;
    private float _pendulumAngle = 230f;
    private float _pendMinAngle;
    private float _pendMaxAngle;

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
        _pendMinAngle = data.minAngle;
        _pendMaxAngle = data.maxAngle;
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

        float finalAngle = _pendulumAngle;
        
        Debug.Log($"Final angle pendulum: {finalAngle}");

        float finalSpeed = Mathf.Max(
            0.05f,
            (finalAngle - _pendMinAngle) * _pendulumDuration /
            (_pendMaxAngle - _pendMinAngle)
        );

        _pendulumSequence = DOTween.Sequence();

        transform.localRotation = Quaternion.Euler(0f, 0f, _pendMinAngle);

        float sweep = _pendMaxAngle - _pendMinAngle;
        
        for (int i = 0; i < _pendulumCycles; i++)
        {
            _pendulumSequence.Append(
                transform.DOLocalRotate(
                    new Vector3(0f, 0f, sweep),
                    _pendulumDuration,
                    RotateMode.LocalAxisAdd
                ).SetEase(Ease.Linear)
            );

            _pendulumSequence.Append(
                transform.DOLocalRotate(
                    new Vector3(0f, 0f, -sweep),
                    _pendulumDuration,
                    RotateMode.LocalAxisAdd
                ).SetEase(Ease.Linear)
            );
        }
        _pendulumSequence.Append(
            transform.DOLocalRotate(
                new Vector3(0f, 0f, _pendMinAngle),
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
                _wobbleDuration,
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

        float totalAngle = (360 * _spins) + _spinAngle;
        
        _spinSequence = DOTween.Sequence();

        _spinSequence.Append(transform.DOLocalRotate(
            new Vector3(0f, 0f, totalAngle),
            _spinDuration,
            RotateMode.FastBeyond360
        ).SetEase(Ease.OutCubic)).
            OnComplete(PublishAngle);

        _spinSequence.Insert(0.7f * _spinDuration, transform.DOPunchRotation(
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
