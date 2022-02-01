using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WorldObjectManager/WorldObjectManager", order = 1)]
public class WorldObjectManager : ScriptableObject
{
    private GameObject worldObjectManager;

    public T GetComponent<T>()
    {
        return worldObjectManager.GetComponent<T>();
    }

    public void SetWorldObjectManager(GameObject gameObject)
    {
        worldObjectManager = gameObject;
    }
}
