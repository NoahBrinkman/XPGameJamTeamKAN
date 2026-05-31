using System.Collections;
using DG.Tweening;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private Transform director;

    [SerializeField] private Transform clown1;
    
    [SerializeField] private Transform clown2;
    
    [SerializeField] private Transform clown3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SceneLoaderManager.Instance?.ResumeOverworldMusic();
        DirectorDance();
        StartCoroutine(YankLoop(clown1, new Vector3(-9.02f, 7.42f, -5.73f), new Vector3(-6.63f, 5.01f, -3.900f),
            5f, 1f, 3f, 1f));
        StartCoroutine(YankLoop(clown2, new Vector3(5.21f,-6.34f,-3.68f), new Vector3(5.21f,-3.20f,-3.68f),
            2f, 0.5f, 2f, 6f));
        StartCoroutine(YankLoop(clown3, new Vector3(9.40f,8.28f,-3.16f), new Vector3(7.05f,5.38f,-3.16f),
            3.5f, 2f, 4.5f, 3.8f));
        
    }

    private void DirectorDance()
    {
        Sequence _pendulumSequence = DOTween.Sequence();
        float sweep = 11.752f + 13.022f;
        
        for (int i = 0; i < 100; i++)
        {
            _pendulumSequence.Append(
                director.DOLocalRotate(
                    new Vector3(0f, 0f, sweep),
                    0.5f,
                    RotateMode.LocalAxisAdd
                ).SetEase(Ease.Linear)
            );
            _pendulumSequence.Join(director.DOJump(new Vector3(-2.49f,-1.42f,-6.55f), 0.1f, 1, 0.5f));
            _pendulumSequence.Append(
                director.DOLocalRotate(
                    new Vector3(0f, 0f, -sweep),
                    0.5f,
                    RotateMode.LocalAxisAdd
                ).SetEase(Ease.Linear)
            );
            _pendulumSequence.Join(director.DOJump(new Vector3(-2.49f,-1.42f,-6.55f), 0.1f, 1, 0.5f));
        }

        
    }

    private IEnumerator YankLoop(Transform clown, Vector3 yankPosition, Vector3 originalPosition,
        float waitAfterYank, float moveBackSpeed, float waitUntilYank, float startDelay)
    {
        for (int i = 0; i < 30; i++)
        {
            yield return new WaitForSeconds(startDelay);
            YankOut(clown, yankPosition);
            yield return new WaitForSeconds(waitAfterYank + 0.8f);

            // Move back slowly
            clown.DOMove(originalPosition, moveBackSpeed)
                .SetEase(Ease.InOutSine);
            yield return new WaitForSeconds(moveBackSpeed);
            yield return new WaitForSeconds(waitUntilYank);
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
