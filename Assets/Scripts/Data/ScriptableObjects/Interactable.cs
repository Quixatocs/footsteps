using System;
using UnityEngine;

[Serializable]
public abstract class Interactable : ScriptableObject
{
    public Sprite sprite;
    public InteractableSpawnChance[] spawnChances;
    
    public virtual Interactable Copy()
    {
        Interactable copiedInteractable = CreateInstance<Interactable>();

        copiedInteractable.sprite = sprite;
        copiedInteractable.spawnChances = spawnChances;
        
        return copiedInteractable;
    }

    public abstract void Interact();
}
