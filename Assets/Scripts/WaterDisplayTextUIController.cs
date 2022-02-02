using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class WaterDisplayTextUIController : MonoBehaviour
{
    private TMP_Text text;
    
    public void UpdateUI(int newValue)
    {
        if (text == null)
        {
            text = GetComponent<TMP_Text>();
        }

        text.text = $"Water: {newValue}";
    }
}
