using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class RaiseArgEventStateNode<T> : StateNode
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference argEventReference;
    [SerializeField]
    private AssetReference variableReference;
    
    private ArgEvent<T> argEvent;
    private Variable<T> variable;

    public override void OnEnter()
    {
        base.OnEnter();

        if (IsInitialised)
        {
            Continue();
            return;
        }

        ++assetLoadCount;
        Addressables.LoadAssetAsync<ArgEvent<T>>(argEventReference).Completed += OnArgEventAssetLoaded;
        ++assetLoadCount;
        Addressables.LoadAssetAsync<Variable<T>>(variableReference).Completed += OnVariableAssetLoaded;
    }
    
    private void OnArgEventAssetLoaded(AsyncOperationHandle<ArgEvent<T>> obj)
    {
        if (obj.Status != AsyncOperationStatus.Succeeded) return;
        
        argEvent = obj.Result;
        Debug.Log($"Successfully loaded asset <{argEvent.name}>");

        ContinueOnAllAssetsLoaded();
    }
    
    private void OnVariableAssetLoaded(AsyncOperationHandle<Variable<T>> obj)
    {
        if (obj.Status != AsyncOperationStatus.Succeeded) return;
        
        variable = obj.Result;
        Debug.Log($"Successfully loaded asset <{variable.name}>");

        ContinueOnAllAssetsLoaded();
    }

    protected override void Continue()
    {
        argEvent.Raise(variable.Value);
        IsComplete = true;
    }
}
