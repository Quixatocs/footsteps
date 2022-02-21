using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[Serializable]
public class Transition
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference NextStateReference; 
    
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

    public void LoadNextStateAsset()
    {
        Addressables.LoadAssetAsync<State>(NextStateReference).Completed += OnNextStateAssetLoaded;
    }

    private void OnNextStateAssetLoaded(AsyncOperationHandle<State> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            nextState = obj.Result;
            Debug.Log($"Successfully loaded asset <{nextState.name}>");
        }
    }

    public State GetNextState()
    {
        if (nextState == null)
        {
            Debug.LogError($"Next State on transition was null");
            return null;
        }
        
        return nextState;
    }
}
