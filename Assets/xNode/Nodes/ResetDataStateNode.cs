﻿using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class ResetDataStateNode<T> : StateNode 
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference variableReference;

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
        Addressables.LoadAssetAsync<Variable<T>>(variableReference).Completed += OnVariableAssetLoaded;
    }
    
    private void OnVariableAssetLoaded(AsyncOperationHandle<Variable<T>> obj)
    {
        if (obj.Status != AsyncOperationStatus.Succeeded) return;
        
        variable = obj.Result;
        Debug.Log($"Successfully loaded asset <{variable}>");

        ContinueOnAllAssetsLoaded();
    }
    
    public override void OnExit()
    {
    }

    protected override void ContinueOnAllAssetsLoaded()
    {
        if (--assetLoadCount != 0) return;
        
        IsInitialised = true;
        Continue();
    }
    
    protected override void Continue()
    {
        variable.Value = variable.DefaultValue;
        IsComplete = true;
    }
}