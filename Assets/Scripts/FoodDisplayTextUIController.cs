using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class FoodDisplayTextUIController : MonoBehaviour
{
    private TMP_Text text;
    
    public void UpdateUI(int newValue)
    {
        if (text == null)
        {
            text = GetComponent<TMP_Text>();
        }

        text.text = $"Food: {newValue}";
    }
}
