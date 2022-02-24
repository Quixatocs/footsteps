using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/States/DeactivateUIState", order = 1)]
public class DeactivateUIState : State
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference UIDeactivationEventReference;
    
    private BoolEvent uiDeactivationEvent;

    public override void OnEnter()
    {
        IsComplete = false;
        Addressables.LoadAssetAsync<BoolEvent>(UIDeactivationEventReference).Completed += OnUIDeactivationEventAssetLoaded;
    }
    
    private void OnUIDeactivationEventAssetLoaded(AsyncOperationHandle<BoolEvent> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            uiDeactivationEvent = obj.Result;
            Debug.Log($"Successfully loaded asset <{uiDeactivationEvent.name}>");

            IsInitialised = true;
            uiDeactivationEvent.Raise(false);
            IsComplete = true;
        }
    }
    
    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }
    
}