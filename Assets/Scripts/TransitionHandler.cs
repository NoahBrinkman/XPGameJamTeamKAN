using System;
using DG.Tweening;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

public class TransitionHandler : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    [SerializeField] private float duration;
    public float Duration { get { return duration; } }
    private void OnEnable()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }
    [ButtonMethod]
    public void FadeToTransparent()
    {
        canvasGroup.alpha = 1;
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0, duration).SetEase(Ease.InQuad);
    }
    [ButtonMethod]
    public void FadeToBlack()
    {
        canvasGroup.alpha = 0;
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1, duration).SetEase(Ease.InQuad);
    }
}
