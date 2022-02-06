using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Map/Entity", order = 1)]
public class Entity : ScriptableObject
{
    public WorldTile[] spawnableTiles; 
    
    public Entity Copy()
    {
        Entity copiedEntity = CreateInstance<Entity>();
        
        copiedEntity.spawnableTiles = spawnableTiles;
        
        return copiedEntity;
    }
}
