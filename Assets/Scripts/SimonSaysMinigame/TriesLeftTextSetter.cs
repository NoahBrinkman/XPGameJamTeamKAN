using TMPro;
using UnityEngine;

public class TriesLeftTextSetter : MonoBehaviour
{
    private TMP_Text t;
    
    public void SetText(int amountLeft)
    {
        if (t == null)
        {
            t= GetComponent<TMP_Text>();
        }    
        t.text = $"Tries left \n {amountLeft}";
    }
}
