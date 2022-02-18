using System;
using UnityEngine;

[Serializable]
public class Transition : ScriptableObject
{
    public State NextState;
    public Comparer Comparer;
    public IntVariable Stat;
    public int Constant;

    public bool IsOpenTransition()
    {
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
}
