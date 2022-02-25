using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class TriggerUIState : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference enableUIStateReference;

    //private EnableUIState enableUIState;
    private Button button;
    
    private void OnEnable()
    {
        //Addressables.LoadAssetAsync<EnableUIState>(enableUIStateReference).Completed += OnEnableUIStateAssetLoaded;
        
        if (button == null)
        {
            button = GetComponent<Button>();
        }
        
        button.interactable = false;
    }

    /*
    private void OnEnableUIStateAssetLoaded(AsyncOperationHandle<EnableUIState> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            enableUIState = obj.Result;
            Debug.Log($"Successfully loaded asset <{enableUIState.name}>");
            
            button.interactable = true;
        }
    }
    */

    public void ReturnFromUI()
    {
        //enableUIState.ReturnFromUI();
    }
}
