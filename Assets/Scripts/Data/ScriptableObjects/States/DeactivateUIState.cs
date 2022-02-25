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
        base.OnEnter();
        
        if (IsInitialised) return;
        
        stateHandleOperation.Completed += OnNextStateAssetLoaded;
        
        ++assetLoadCount;
        Addressables.LoadAssetAsync<BoolEvent>(UIDeactivationEventReference).Completed += OnUIDeactivationEventAssetLoaded;
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
                uiDeactivationEvent.Raise(false);
                IsComplete = true;
            }
        }
    }
    
    private void OnUIDeactivationEventAssetLoaded(AsyncOperationHandle<BoolEvent> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            --assetLoadCount;
            uiDeactivationEvent = obj.Result;
            Debug.Log($"Successfully loaded asset <{uiDeactivationEvent.name}>");

            if (assetLoadCount == 0)
            {
                IsInitialised = true;
                uiDeactivationEvent.Raise(false);
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