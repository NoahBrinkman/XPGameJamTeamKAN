using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Transform director;
    [SerializeField] private Transform clown1;
    [SerializeField] private Transform clown2;
    [SerializeField] private Transform clown3;
    [SerializeField] private Transform clown4;
    [SerializeField] private Transform clown5;
    [SerializeField] private Transform pedestal;
    [SerializeField] private Transform car;

    [SerializeField] private List<TMP_Text> texts;
    [SerializeField] private List<Image> images;
    [SerializeField] private Image widePanel;
    [SerializeField] private TMP_Text storyText;
    [SerializeField] private TMP_Text signature;

    [SerializeField] private Transform directorGoal;
    

    public void Story()
    {
        Sequence directorMove = DOTween.Sequence();
        directorMove.Append(director.DORotate(directorGoal.rotation.eulerAngles, 3f));
        directorMove.Join(director.DOJump(directorGoal.position, 0.1f, 12, 4f));
        directorMove.Join(widePanel.DOFade(1, 3f));
        directorMove.Append(storyText.DOFade(1, 2f));
        directorMove.Join(signature.DOFade(1, 2f));
        
        YankOut(clown1, new Vector3(0.1f, 8.69f, -4f));
        YankOut(clown2, new Vector3(5.1f,-8f,-2.4f));

        pedestal.DOMove(new Vector3(12.18f, -2.1f, -4.04f), 3f);
        pedestal.DORotate(new Vector3(0, 540, 0), 3f, RotateMode.FastBeyond360);

        foreach (var text in texts)
        {
            text.DOFade(0, 1.5f).OnComplete(()=> text.gameObject.SetActive(false));
        }
        
        foreach (var image in images)
        {
            image.DOFade(0, 1.5f).OnComplete(() => image.gameObject.SetActive(false));
        }

    }

    private void YankOut(Transform clown, Vector3 newPosition)
    {
        Sequence clownCeiling = DOTween.Sequence();
        clownCeiling.Append(
            clown.DOScale(
                new Vector3(1.08f, 1.12f, 1.08f),
                0.15f
            ).SetEase(Ease.OutQuad)
        );

        clownCeiling.Append(
            clown.DOMove(
                newPosition,
                0.8f
            ).SetEase(Ease.InExpo)
        );

        clownCeiling.Join(
            clown.DOScale(
                new Vector3(0.95f, 1.15f, 0.95f),
                0.3f
            ).SetLoops(2, LoopType.Yoyo)
        );
    }
}
