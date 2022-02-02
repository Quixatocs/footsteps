using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WorldManagers/UIObjectManager", order = 1)]
public class UIObjectManager : ScriptableObject
{
    private GameObject uiObjectManager;

    public T GetUIController<T>()
    {
        return uiObjectManager.GetComponent<T>();
    }

    public void SetUIObjectManager(GameObject gameObject)
    {
        uiObjectManager = gameObject;
    }
}
