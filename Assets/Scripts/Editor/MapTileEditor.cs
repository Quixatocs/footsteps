using System;
using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(MapTile), true)]
public class MapTileEditor : Editor
{
    //private SerializedProperty name;
    private SerializedProperty tile;
    private SerializedProperty tileNeighbourWeight;
    
    private void OnEnable()
    {
        //name = serializedObject.FindProperty("name");
        tile = serializedObject.FindProperty("tile");
        tileNeighbourWeight = serializedObject.FindProperty("tileNeighbourWeight");
    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        //EditorGUILayout.PropertyField(name);
        EditorGUILayout.PropertyField(tile);
        EditorGUILayout.PropertyField(tileNeighbourWeight);
        
        serializedObject.ApplyModifiedProperties();
    }
}
