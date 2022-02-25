using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class ButtonReferenceLoader : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference uiStateReference;

    //private EnableUIState enableUIState;
    private Button button;

    private void OnEnable()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
        
        //Addressables.LoadAssetAsync<EnableUIState>(uiStateReference).Completed += OnEnableUIStateAssetLoaded;
    }

    /*
    private void OnEnableUIStateAssetLoaded(AsyncOperationHandle<EnableUIState> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            enableUIState = obj.Result;
            Debug.Log($"Successfully loaded asset <{enableUIState.name}>");
            
            button.onClick.AddListener(enableUIState.ReturnFromUI);
        }
    }
    */

    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
}
