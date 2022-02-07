
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Map/CreatureInteractable", order = 1)]
public class CreatureInteractable : Interactable
{
    public IntDelta harvestable;

    public override Interactable Copy()
    {
        CreatureInteractable copiedCreatureInteractable = base.Copy() as CreatureInteractable;
        copiedCreatureInteractable.harvestable = harvestable;

        return copiedCreatureInteractable;
    }

    public override void Interact()
    {
        harvestable.ApplyDelta();
    }
}
