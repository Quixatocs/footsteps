using System;
using UnityEngine;

[Serializable]
public abstract class Interactable : ScriptableObject
{
    public Sprite sprite;
    public InteractableSpawnChance[] spawnChances;
    
    [NonSerialized]
    public GameObject MapIcon;

    public abstract Interactable Copy();

    public virtual void Interact()
    {
        Destroy(MapIcon);
    }
}
