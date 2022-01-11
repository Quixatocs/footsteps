using System;
using UnityEngine;
using UnityEngine.WSA;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/New MapTile", order = 1)]
[Serializable]
public class MapTile : ScriptableObject
{
    public string name;
    public Sprite sprite;
    public Color color;
    public TileNeighbourWeight[] tileNeighbourWeight;
}
