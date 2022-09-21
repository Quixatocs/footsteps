using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

[Serializable]
public class Transition
{
    [FormerlySerializedAs("NextStateReference")]
    [Header("Asset References")]
    [SerializeField]
    private AssetReference IntVariableReference; 
    
    [Header("Comparison References")]
    public IntVariable Stat;
    public Comparer Comparer;
    public int Constant;

    private State nextState;

    public bool IsOpenTransition()
    {
        if (Stat == null)
        {
            Debug.LogError($"Stat on transition was null");
            return false;
        }
        
        switch (Comparer)
        {
            case Comparer.Equals:
                return Stat.Value == Constant;
            case Comparer.LessThan:
                return Stat.Value < Constant;
            case Comparer.GreaterThan:
                return Stat.Value > Constant;
            default:
                Debug.LogError($"Comparator value of <{Comparer}> was unexpected");
                return false;
        } 
    }

    public AsyncOperationHandle<State> LoadNextStateAsset()
    {
        AsyncOperationHandle<State> op = IntVariableReference.LoadAssetAsync<State>();
        op.Completed += OnLoadNextStateAssetComplete;
        return op;
    }
    
    private void OnLoadNextStateAssetComplete(AsyncOperationHandle<State> obj)
    {
        if (obj.Status != AsyncOperationStatus.Succeeded) return;
        
        nextState = obj.Result;
        Debug.Log($"Successfully loaded asset <{nextState.name}>");
    }

    public State GetNextState()
    {
        if (nextState != null) return nextState;
        
        Debug.LogError($"Next State on transition was null");
        return null;

    }
}
