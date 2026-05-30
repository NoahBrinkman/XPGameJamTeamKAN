using System;
using System.Collections.Generic;
using DG.Tweening;
using MyBox;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalManager : MonoBehaviour
{
    [SerializeField] private List<AnimalAndCurveList> animals = new List<AnimalAndCurveList>();
    [SerializeField] private RectTransform targetImage;
    [SerializeField] private float startPosX = 15;
    [SerializeField] private float endPosX = 380;
    [SerializeField] private float duration = 7;
    private Sequence sequence;
    
    [ButtonMethod]
    public void ChooseCurve()
    {
        var selection = animals[Random.Range(0, animals.Count)];
        selection.animalObject.SetActive(true);
       sequence = DOTween.Sequence().Append(targetImage.DOAnchorPosX(endPosX, duration).SetEase(selection.curves[Random.Range(0, selection.curves.Count)]));
    }

    public void OnComplete()
    {
        sequence.Kill();
    }

}

[Serializable]
public class AnimalAndCurveList
{
    public GameObject animalObject;
    public List<AnimationCurve> curves;
}

