using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

[Serializable]
public class Transition
{
    
    [Header("Asset References")]
    [SerializeField]
    private AssetReference intVariableReference; 
    
    [Header("Comparison References")]
    private IntVariable stat;
    [SerializeField]
    private Comparer comparer;
    [SerializeField]
    private int constant;

    public bool IsOpenTransition()
    {
        if (stat == null)
        {
            Debug.LogError($"Stat on transition was null");
            return false;
        }
        
        switch (comparer)
        {
            case Comparer.Equals:
                return stat.Value == constant;
            case Comparer.LessThan:
                return stat.Value < constant;
            case Comparer.GreaterThan:
                return stat.Value > constant;
            default:
                Debug.LogError($"Comparator value of <{comparer}> was unexpected");
                return false;
        } 
    }

    public AsyncOperationHandle<IntVariable> LoadLogicAsset()
    {
        AsyncOperationHandle<IntVariable> op = intVariableReference.LoadAssetAsync<IntVariable>();
        op.Completed += OnLoadLogicAssetComplete;
        return op;
    }
    
    private void OnLoadLogicAssetComplete(AsyncOperationHandle<IntVariable> obj)
    {
        if (obj.Status != AsyncOperationStatus.Succeeded) return;
        
        stat = obj.Result;
        Debug.Log($"Successfully loaded asset <{stat.name}>");
    }
}
