using System;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class MinimapHandler : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private RectTransform minimapParent;
    [SerializeField] private float minimapResolution;
    [SerializeField] private Transform bottomLeft;
    [SerializeField] private Transform topRight;
    [SerializeField] private Transform mapRevealerInGame;
    [SerializeField] private Transform mapRevealerInUI;
    private List<GameObject> mapTiles;
    [SerializeField]private float distanceRequiredToReveal = 1;

    public void GenerateMap()
    { 
        mapTiles = new List<GameObject>();
        
        var minimapParentWidth = minimapParent.rect.width;
      
        var tileSize = minimapParentWidth / minimapResolution;
        for (int i = 0; i < minimapResolution; i++)
        {
            for (int j = 0; j < minimapResolution; j++)
            {  var tile = Instantiate(tilePrefab, minimapParent);

                RectTransform rectTransform = tile.GetComponent<RectTransform>();

                rectTransform.SetWidth(tileSize);
                rectTransform.SetHeight(tileSize);

                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);

                float posX = (-minimapParentWidth * 0.5f) + (tileSize * 0.5f) + (i * tileSize);
                float posY = (-minimapParentWidth * 0.5f) + (tileSize * 0.5f) + (j * tileSize);

                rectTransform.anchoredPosition = new Vector2(posX, posY);

                mapTiles.Add(tile);
                
            }
        }
        mapRevealerInUI.SetSiblingIndex(minimapParent.childCount - 1);
    }

    private void Start()
    {
        if(mapTiles == null || mapTiles.Count <= 0)
            GenerateMap();
    }

    private void LateUpdate()
    {
        UpdateMap();
    }

    public void UpdateMap()
    {
        //Set UI Position of UI Revealer to relative position of the non-UI revealer
        // Convert the world position of the revealer into a 0-1 range
        float normalizedX = Mathf.InverseLerp(bottomLeft.position.x, topRight.position.x, mapRevealerInGame.position.x);
        float normalizedY = Mathf.InverseLerp(bottomLeft.position.z, topRight.position.z, mapRevealerInGame.position.z);

        // Parent is always a square
        float parentSize = minimapParent.rect.width;

        // Convert normalized position into UI local space
        float uiX = (normalizedX * parentSize) - (parentSize * 0.5f);
        float uiY = (normalizedY * parentSize) - (parentSize * 0.5f);

        Vector3 pointInMap = new Vector3(uiX, uiY, 0f);

        // Move UI revealer
        mapRevealerInUI.localPosition = pointInMap;
        
        
        foreach (var tile in mapTiles)
        {
            if (Vector3.Distance(tile.transform.localPosition, mapRevealerInUI.localPosition) < distanceRequiredToReveal)
            {
                tile.gameObject.SetActive(false);
            }
        }
    }

}
