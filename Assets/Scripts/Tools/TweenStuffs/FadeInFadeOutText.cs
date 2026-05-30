using DG.Tweening;
using TMPro;
using UnityEngine;

public class FadeInFadeOutText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float duration = 1.4f;

    public void FadeIn()
    {
        text.DOFade(1, duration);
    }

    public void FadeOut()
    {
        text.DOFade(0, duration);
    }
}
