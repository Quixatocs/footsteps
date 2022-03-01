using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WorldManagers/WorldObjectManager", order = 1)]
public class WorldObjectManager : ScriptableObject
{
    private GameObject worldObjectManager;
    private List<WorldTile> worldTiles;

    public T GetComponent<T>()
    {
        return worldObjectManager.GetComponent<T>();
    }

    public void SetWorldObjectManager(GameObject gameObject)
    {
        worldObjectManager = gameObject;
    }

    public void SetWorldTiles(List<WorldTile> worldTiles)
    {
        this.worldTiles = worldTiles;
    }
}
