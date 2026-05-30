using DG.Tweening;
using UnityEngine;

public class City : MonoBehaviour
{
    [SerializeField] private GameObject normalMesh;
    [SerializeField] private GameObject circusMesh;
    private Sequence _sequence;
    [SerializeField] private GameObject fireworks;
    public void MeshSwap()
    {
        /*_sequence = DOTween.Sequence().Append(normalMesh.transform.DOScale(Vector3.zero, .5f))
            .OnComplete(() =>
            {
                circusMesh.SetActive(true);
                normalMesh.SetActive(false);
            }).Append(circusMesh.transform.DOScale(Vector3.one, .5f)); */
        normalMesh.SetActive(false);
        circusMesh.SetActive(true);
        fireworks.SetActive(true);
    }
}
