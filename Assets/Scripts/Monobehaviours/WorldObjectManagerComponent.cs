using UnityEngine;

public class WorldObjectManagerComponent : MonoBehaviour
{
    public WorldObjectManager WorldObjectManager;

    private void Start()
    {
        WorldObjectManager.SetWorldObjectManager(gameObject);
    }
}
