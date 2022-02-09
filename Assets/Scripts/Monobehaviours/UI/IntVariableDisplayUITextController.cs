using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(TMP_Text))]
public class IntVariableDisplayUITextController : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference PlayerStatReference;
    
    [Header("Variables")]
    public IntVariable PlayerStat;
    
    [Header("Component Properties")]
    public string prefix;
    
    private TMP_Text text;
    
    private void Awake()
    {
        Addressables.LoadAssetAsync<IntVariable>(PlayerStatReference).Completed += OnAssetLoaded;
    }

    private void OnAssetLoaded(AsyncOperationHandle<IntVariable> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            PlayerStat = obj.Result;
            Debug.Log($"Successfully loaded asset <{PlayerStat.name}>.");
            UpdateUI();
        }
    }
    
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
