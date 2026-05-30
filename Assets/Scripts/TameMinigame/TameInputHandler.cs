using System;
using EventBus;
using MyBox;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TameInputHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image fillImage;

    [ReadOnly, SerializeField] private bool cursorInField = false;
    [SerializeField] private float fillSpeed;
    [SerializeField] private float emptySpeed = .3f;
    private float fillAmount = 0;
    [SerializeField] private float gameDuration = 10;
    private float gameTimer = 10f;
    public bool gameStarted = false;
    private bool startedAudioUp = false;
    [SerializeField] private AudioSource sfxSourceUp;
    
    [SerializeField] private AudioSource sfxSourceDown;
    [SerializeField] private FloatPublisher finishGamePublisher;
    
    private void Update()
    {
        if (gameStarted)
        {
            gameTimer -= Time.deltaTime;
            if (cursorInField)
            {
                if (!startedAudioUp)
                {
                    sfxSourceUp.Play();
                    startedAudioUp = true;
                }
                else if (!sfxSourceUp.isPlaying)
                {
                    sfxSourceUp.Play();
                    sfxSourceDown.Stop();
                }
                fillAmount += fillSpeed * Time.deltaTime;
                fillAmount = Mathf.Clamp(fillAmount,0, 1 );
                if (fillAmount >= 1)
                {
                    //early game over cuz you won!
                    gameStarted = false;
                    finishGamePublisher.SetValueAndPublish(fillAmount);
                }
            }
            else
            {
                if (startedAudioUp && !sfxSourceDown.isPlaying)
                {
                    sfxSourceUp.Stop();
                    sfxSourceDown.Play();
                }
              
                fillAmount -= emptySpeed * Time.deltaTime;
                fillAmount =  Mathf.Clamp(fillAmount,0, 1 );
            }
            fillImage.fillAmount = Mathf.Lerp(0, 1, fillAmount);
            if (gameTimer <= 0)
            {
                //Game over man!
                gameStarted = false;
                sfxSourceUp.Stop();
                sfxSourceDown.Stop();
                finishGamePublisher.SetValueAndPublish(fillAmount);
            }
        }
    }

    
    public void StartGame()
    {
        gameStarted = true;
        startedAudioUp = false;
        gameTimer = gameDuration;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(gameStarted)
            cursorInField = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(gameStarted)
            cursorInField = false;
    }
}
