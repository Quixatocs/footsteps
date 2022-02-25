using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/States/ActivateUIState", order = 1)]
public class ActivateUIState : State
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference uiActivationEventReference;
    
    private BoolEvent uiActivationEvent;

    public override void OnEnter()
    {
        base.OnEnter();
        
        if (IsInitialised) return;
        
        stateHandleOperation.Completed += OnNextStateAssetLoaded;
        
        ++assetLoadCount;
        Addressables.LoadAssetAsync<BoolEvent>(uiActivationEventReference).Completed += OnUIActivationEventAssetLoaded;
    }
    
    private void OnNextStateAssetLoaded(AsyncOperationHandle<State> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            --assetLoadCount;
            nextState = obj.Result;
            Debug.Log($"Successfully loaded asset <{nextState.name}>");

            if (assetLoadCount == 0)
            {
                IsInitialised = true;
                uiActivationEvent.Raise(true);
                IsComplete = true;
            }
        }
    }
    
    private void OnUIActivationEventAssetLoaded(AsyncOperationHandle<BoolEvent> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            --assetLoadCount;
            uiActivationEvent = obj.Result;
            Debug.Log($"Successfully loaded asset <{uiActivationEvent.name}>");

            if (assetLoadCount == 0)
            {
                IsInitialised = true;
                uiActivationEvent.Raise(true);
                IsComplete = true; 
            }
        }
    }
    
    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }
    
    public override State GetNextState()
    {
        return nextState;
    }
    
}