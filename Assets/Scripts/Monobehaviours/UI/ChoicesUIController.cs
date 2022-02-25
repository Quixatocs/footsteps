using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ChoicesUIController : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference worldObjectManagerReference;
    [SerializeField]
    private AssetReference playerCurrentHexReference;
    [SerializeField]
    private AssetReference choiceClickedEventReference;
    
    [Header("Scene References")]
    [SerializeField]
    private GameObject choicesButtonPrefab;
    [SerializeField]
    private GameObject buttonHolderParent;

    private WorldObjectManager worldObjectManager;
    private HexVariable playerCurrentHex; 
    private VoidEvent choiceClickedEvent;
    private Tilemap tileMap;
    private readonly List<GameObject> buttonGOs = new List<GameObject>();

    private bool isInitialised;
    private int assetLoadCount;
    
    private void OnEnable()
    {
        if (!isInitialised)
        {
            ++assetLoadCount;
            Addressables.LoadAssetAsync<WorldObjectManager>(worldObjectManagerReference).Completed += OnWorldObjectManagerAssetLoaded;
            ++assetLoadCount;
            Addressables.LoadAssetAsync<HexVariable>(playerCurrentHexReference).Completed += OnPlayerCurrentHexAssetLoaded;
            ++assetLoadCount;
            Addressables.LoadAssetAsync<VoidEvent>(choiceClickedEventReference).Completed += OnChoiceClickedEventAssetLoaded;
        }
        else
        {
            SetUpButtons();
        }
    }

    private void OnWorldObjectManagerAssetLoaded(AsyncOperationHandle<WorldObjectManager> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            worldObjectManager = obj.Result;
            Debug.Log($"Successfully loaded asset <{worldObjectManager.name}>");

            if (tileMap == null)
            {
                tileMap = worldObjectManager.GetComponent<Tilemap>();
            }
            
            ContinueOnAllAssetsLoaded();
        }
    }
    
    private void OnPlayerCurrentHexAssetLoaded(AsyncOperationHandle<HexVariable> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            playerCurrentHex = obj.Result;
            Debug.Log($"Successfully loaded asset <{playerCurrentHex.name}>");
            
            ContinueOnAllAssetsLoaded();
        }
    }
    
    private void OnChoiceClickedEventAssetLoaded(AsyncOperationHandle<VoidEvent> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            choiceClickedEvent = obj.Result;
            Debug.Log($"Successfully loaded asset <{choiceClickedEvent.name}>");

            ContinueOnAllAssetsLoaded();
        }
    }

    private void ContinueOnAllAssetsLoaded()
    {
        if (--assetLoadCount == 0)
        {
            isInitialised = true;
            SetUpButtons();
        }
    }

    private void SetUpButtons()
    {
        buttonGOs.Clear();
        
        WorldTile currentTile = (WorldTile)tileMap.GetTile(playerCurrentHex.Value);
        IntDelta[] currentTileHarvestables = currentTile.harvestables;
        
        foreach (IntDelta harvestable in currentTileHarvestables)
        {
            GameObject buttonGO = Instantiate(choicesButtonPrefab, buttonHolderParent.transform);
            buttonGOs.Add(buttonGO);
            Button button = buttonGO.GetComponent<Button>();
            button.onClick.AddListener(() =>
                {
                    harvestable.ApplyDelta();
                    choiceClickedEvent.Raise();
                });
            TMP_Text buttonText = buttonGO.GetComponentInChildren<TMP_Text>();
            
            StringBuilder buttonTextBuilder = new StringBuilder();
            buttonTextBuilder.Append(harvestable.stat.name)
                .Replace("(IntVariable)", "")
                .Append(" : ")
                .Append(harvestable.baseAmount);
                
            buttonText.text = buttonTextBuilder.ToString();
        }

        foreach (Interactable interactable in currentTile.runtimeInteractables)
        {
            GameObject buttonGO = Instantiate(choicesButtonPrefab, buttonHolderParent.transform);
            buttonGOs.Add(buttonGO);
            Button button = buttonGO.GetComponent<Button>();
            button.onClick.AddListener(() =>
                {
                    interactable.Interact();
                    currentTile.runtimeInteractables.Remove(interactable);
                    choiceClickedEvent.Raise();
                }
            );
            TMP_Text buttonText = buttonGO.GetComponentInChildren<TMP_Text>();
            
            StringBuilder buttonTextBuilder = new StringBuilder();
            buttonTextBuilder.Append(interactable.name);
                
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
