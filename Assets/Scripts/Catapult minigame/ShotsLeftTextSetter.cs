using TMPro;
using UnityEngine;

public class ShotsLeftTextSetter : MonoBehaviour
{
    private TMP_Text t;
    
    public void SetText(int amountLeft)
    {
        if (t == null)
        {
            t= GetComponent<TMP_Text>();
        }    
        t.text = $"Shots left: {amountLeft}/5";
    }
}
