using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class TriggerIntDelta : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference intDeltaReference;

    private IntDelta intDelta;
    private Button button;
    
    private void OnEnable()
    {
        Addressables.LoadAssetAsync<IntDelta>(intDeltaReference).Completed += OnIntDeltaAssetLoaded;

        if (button == null)
        {
            button = GetComponent<Button>();
        }
        
        button.interactable = false;
    }

    private void OnIntDeltaAssetLoaded(AsyncOperationHandle<IntDelta> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            intDelta = obj.Result;
            Debug.Log($"Successfully loaded asset <{intDelta.name}>");
            
            button.interactable = true;
        }
    }

    public void ApplyDelta()
    {
        intDelta.ApplyDelta();
    }
}
