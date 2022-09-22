using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Map/WorldTileSet", order = 1)]
[Serializable]
public class WorldTileSet : ScriptableObject
{
    public AssetReference[] WorldTiles;
    
    //TODO enforce this as a Set
}
