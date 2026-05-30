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
    [SerializeField] private TMP_Text compassTxt2;
    [SerializeField] private TMP_Text driveText;
    [SerializeField] private TMP_Text powerupText;
    [SerializeField] private GameObject powerBtn;
    [SerializeField] private TMP_Text cityTxt;

    private float _fadeDuration = 1.5f;
    private int _spinCount;

    private void Awake()
    {
        city.SetActive(false);
        car.SetActive(false);
        compass.SetActive(false);
        compassBtn.SetActive(false);
        powerBtn.SetActive(false);
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
        compassText.DOFade(1, _fadeDuration).OnComplete(()=> StartCoroutine(WaitBeforeCompassBtn()));
    }

    private void ShowCompassBtn()
    {
        compassBtn.SetActive(true);
        compassTxt.DOFade(1, _fadeDuration);
    }

    public void ShowPowerBtn()
    {
        powerBtn.SetActive(true);
        driveText.DOFade(0, _fadeDuration);
        powerupText.DOFade(1, _fadeDuration);
    }

    public void ShowCityText()
    {
        powerupText.DOFade(0, _fadeDuration);
        cityTxt.DOFade(1, _fadeDuration);
    }

    public void Show()
    {
        if (_spinCount == 0)
        {
            compassTxt.DOFade(0, _fadeDuration);
            compassTxt2.DOFade(1, _fadeDuration);
            _spinCount = 1;
        }
        else if (_spinCount == 1)
        {
            compassTxt2.DOFade(0, _fadeDuration);
            driveText.DOFade(1, _fadeDuration);
            _spinCount = 2;
        }
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
        sequence.Append(compassText.DOFade(0, _fadeDuration)).OnComplete(()=>ShowCompassBtn());
    }
}
