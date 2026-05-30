using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] private GameObject city;
    [SerializeField] private TMP_Text carText;
    [SerializeField] private GameObject compass;
    [SerializeField] private TMP_Text compassText;
    [SerializeField] private GameObject compassBtn;
    [SerializeField] private TMP_Text compassTxt;

    private float _fadeDuration;

    private void Awake()
    {
        city.SetActive(false);
        car.SetActive(false);
        compassBtn.SetActive(false);
    }

    private void Start()
    {
        ShowCar();
        StartCoroutine(WaitBeforeCompass());
    }

    private void ShowCar()
    {
        car.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        car.SetActive(true);
        car.transform.DOScale(new Vector3(1, 1, 1), 1f);
        carText.DOFade(1, _fadeDuration);
    }

    private void ShowCompass()
    {
        compass.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        compass.SetActive(true);
        compass.transform.DOScale(new Vector3(1, 1, 1), 1f);
        compassText.DOFade(1, _fadeDuration);
    }

    private void ShowCompassBtn()
    {
        
    }
    
    
    public void ActivateCity()
    {
        city.SetActive(true);
    }

    IEnumerator WaitBeforeCompass()
    {
        yield return new WaitForSeconds(4);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(carText.DOFade(0, _fadeDuration)).OnComplete(()=>ShowCompass());
    }

    IEnumerator WaitBeforeCompassBtn()
    {
        yield return new WaitForSeconds(4);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(compassText.DOFade(0, _fadeDuration)).OnComplete(()=>ShowCompass());
    }
}
