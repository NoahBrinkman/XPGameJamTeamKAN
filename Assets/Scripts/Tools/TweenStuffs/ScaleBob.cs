using DG.Tweening;
using UnityEngine;

public class ScaleBob : MonoBehaviour
{
    [SerializeField] private float to;
    [SerializeField] private float duration;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(new Vector3(to,to,to), duration));
        sequence.SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
