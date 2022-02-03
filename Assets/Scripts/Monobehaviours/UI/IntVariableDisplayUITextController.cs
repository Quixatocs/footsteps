using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class IntVariableDisplayUITextController : MonoBehaviour
{
    private TMP_Text text;
    public IntVariable PlayerStat;
    public string prefix;
    
    private void OnEnable()
    {
        UpdateUI();
    }
    
    public void UpdateUI()
    {
        if (text == null)
        {
            text = GetComponent<TMP_Text>();
        }

        text.text = $"{prefix}: {PlayerStat.Value}";
    }
}
