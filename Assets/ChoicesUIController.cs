using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ChoicesUIController : MonoBehaviour
{
    public WorldObjectManager WorldObjectManager;
    [SerializeField]
    private GameObject choicesButtonPrefab;
    [SerializeField]
    private GameObject buttonHolderParent;

    public HexVariable playerCurrentHex; 

    private Tilemap tileMap;

    private List<GameObject> buttonGOs;
    
    private void OnEnable()
    {
        buttonGOs = new List<GameObject>();
        
        if (tileMap == null)
        {
            tileMap = WorldObjectManager.GetComponent<Tilemap>();
        }
        
        WorldTile currentTile = (WorldTile)tileMap.GetTile(playerCurrentHex.Value);
        IntDelta[] currentTileHarvestables = currentTile.harvestables;
        
        foreach (IntDelta harvestable in currentTileHarvestables)
        {
            GameObject buttonGO = Instantiate(choicesButtonPrefab, buttonHolderParent.transform);
            buttonGOs.Add(buttonGO);
            Button button = buttonGO.GetComponent<Button>();
            button.onClick.AddListener(() => harvestable.ApplyDelta());
            TMP_Text buttonText = buttonGO.GetComponentInChildren<TMP_Text>();
            
            StringBuilder buttonTextBuilder = new StringBuilder();
            buttonTextBuilder.Append(harvestable.stat.name)
                .Replace("(IntVariable)", "")
                .Append(" : ")
                .Append(harvestable.baseAmount);
                
            buttonText.text = buttonTextBuilder.ToString();
        }
    }

    private void OnDisable()
    {
        foreach (GameObject GO in buttonGOs)
        {
            Destroy(GO);
        }
        buttonGOs.Clear();
    }
}
