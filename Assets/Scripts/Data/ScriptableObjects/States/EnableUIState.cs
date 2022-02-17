using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/States/EnableUIState", order = 1)]
public class EnableUIState : State
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference UIActivationEventReference;
    
    private BoolEvent uiActivationEvent;

    public override void OnEnter()
    {
        IsComplete = false;
        Addressables.LoadAssetAsync<State>(NextStateReference).Completed += OnNextStateAssetLoaded;
        Addressables.LoadAssetAsync<BoolEvent>(UIActivationEventReference).Completed += OnUIActivationEventAssetLoaded;
    }
    
    private void OnNextStateAssetLoaded(AsyncOperationHandle<State> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            nextState = obj.Result;
            Debug.Log($"Successfully loaded asset <{nextState.name}>");

            if (uiActivationEvent != null)
            {
                IsInitialised = true;
                uiActivationEvent.Raise(true);
            }
        }
    }
    
    private void OnUIActivationEventAssetLoaded(AsyncOperationHandle<BoolEvent> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            uiActivationEvent = obj.Result;
            Debug.Log($"Successfully loaded asset <{uiActivationEvent.name}>");

            if (nextState != null)
            {
                IsInitialised = true;
                uiActivationEvent.Raise(true);
            }
        }
    }
    
    public override void OnExit()
    {
        uiActivationEvent.Raise(false);
    }

    public override void OnUpdate()
    {
    }

    public void ReturnFromUI()
    {
        IsComplete = true;
    }
    
}
