using System;
using UnityEngine;

[Serializable]
public abstract class Interactable : ScriptableObject
{
    public Sprite sprite;
    public InteractableSpawnChance[] spawnChances;
    
    [NonSerialized]
    public GameObject MapIcon;
    
    public virtual Interactable Copy()
    {
        Interactable copiedInteractable = CreateInstance<Interactable>();

        copiedInteractable.sprite = sprite;
        copiedInteractable.spawnChances = spawnChances;
        
        return copiedInteractable;
    }

    public virtual void Interact()
    {
        Destroy(MapIcon);
    }
}
