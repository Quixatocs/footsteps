
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Map/CreatureInteractable", order = 1)]
public class CreatureInteractable : Interactable
{
    public IntDelta Harvestable;

    public override Interactable Copy()
    {
        CreatureInteractable copiedCreatureInteractable = base.Copy() as CreatureInteractable;
        copiedCreatureInteractable.Harvestable = Harvestable;

        return copiedCreatureInteractable;
    }

    public override void Interact()
    {
        Harvestable.ApplyDelta();
        base.Interact();
    }
}
