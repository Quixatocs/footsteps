using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameController : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference stateMachineReference;
    [SerializeField]
    private AssetReference initialStateReference;
    
    private StateMachine stateMachine;
    private State initialState;

    private bool isInitialised;

    private void Start()
    {
        isInitialised = false;
        
        Addressables.LoadAssetAsync<StateMachine>(stateMachineReference).Completed += OnStateMachineLoaded;
        Addressables.LoadAssetAsync<State>(initialStateReference).Completed += OnInitialStateInitialStateLoaded;
    }
    
    private void OnStateMachineLoaded(AsyncOperationHandle<StateMachine> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            stateMachine = obj.Result;
            Debug.Log($"Successfully loaded asset <{stateMachine.name}>");
            
            if (initialState != null)
            {
                stateMachine.SetState(initialState);
                isInitialised = true;
            }
        }
    }
    
    private void OnInitialStateInitialStateLoaded(AsyncOperationHandle<State> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            initialState = obj.Result;
            Debug.Log($"Successfully loaded asset <{initialState.name}>");

            if (stateMachine != null)
            {
                stateMachine.SetState(initialState);
                isInitialised = true;
            }
        }
    }

    private void Update()
    {
        if (isInitialised)
        {
            stateMachine.CheckCurrentStateCompleted();
            stateMachine.StateUpdate();
        }
        
    }
}
