
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Map/CreatureInteractable", order = 1)]
public class CreatureInteractable : Interactable
{
    public IntDelta Harvestable;

    public override Interactable Copy()
    {
        CreatureInteractable copiedInteractable = CreateInstance<CreatureInteractable>();
        copiedInteractable.sprite = sprite;
        copiedInteractable.spawnChances = spawnChances;
        copiedInteractable.Harvestable = Harvestable;

        return copiedInteractable;
    }

    public override void Interact()
    {
        base.Interact();
        Harvestable.ApplyDelta();
    }
}
