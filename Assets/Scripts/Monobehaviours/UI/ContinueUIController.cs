using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ContinueUIController : MonoBehaviour
{
    public WorldObjectManager WorldObjectManager;
    [SerializeField]
    private GameObject continueButtonPrefab;
    [SerializeField]
    private GameObject canvas;

    public HexVariable playerCurrentHex; 

    private Tilemap tileMap;

    private GameObject buttonHolder;
    
    private void OnEnable()
    {
        buttonHolder = Instantiate(continueButtonPrefab, canvas.transform);
        Button button = buttonHolder.GetComponent<Button>();
        if (tileMap == null)
        {
            tileMap = WorldObjectManager.GetComponent<Tilemap>();
        }
        
        WorldTile currentTile = (WorldTile)tileMap.GetTile(playerCurrentHex.Value);
        IntDelta[] currentTileCosts = currentTile.costs;
        
        foreach (IntDelta cost in currentTileCosts)
        {
            button.onClick.AddListener(() => cost.ApplyDelta());
        }
    }

    private void OnDisable()
    {
        Destroy(buttonHolder);
    }
    
}
