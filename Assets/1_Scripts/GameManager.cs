using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum ItemType
{
    Sprite,
    Text,
    Color
}

public class GameManager : MonoBehaviour
{
    public int width=6;
    public int height=9;
    
    public static GameManager instance;
    public ItemType itemType;

    public Sprite[] spriteItems;
    public String[] strings;
    public Color[] colors;
    [Space(5)]
    public bool isGamePlayable=true;
    public List<Tile> openTileList = new List<Tile>();

    public static event Action OnGameOver;

    public int disabledTiles;
    public ParticleSystem starParticle;
    public UIManager _uiManager;
    
    void Awake()
    {
        instance = this;
        
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
  Debug.unityLogger.logEnabled = false;
#endif
        
    }

    private void OnEnable()
    {
        disabledTiles = width * height;

        _uiManager = GetComponent<UIManager>();
    }

    public void SetItemToTile(Tile tile, int groupId, int indexInGroup)
    {
        tile.isPlayable = false;
      
        tile.itemSprite.enabled = false;
        tile.textMeshPro.enabled = false;

        tile.itemSprite.color=Color.white;

        switch (itemType)
        {
            case ItemType.Sprite:
                
                tile.itemSprite.sortingOrder = 1;
                tile.itemSprite.sprite = spriteItems[groupId];
                
                EnabledItemInGroup(indexInGroup, groupId, tile, true);
                
                break;
            
            case ItemType.Text:
                
                tile.itemSprite.sortingOrder = -1;
                tile.textMeshPro.text = strings[groupId].ToString();
                
                EnabledItemInGroup(indexInGroup, groupId, tile, false);
                
                break;
            
            case ItemType.Color:
               
                tile.itemSprite.sortingOrder = 1;
                tile.itemSprite.color = colors[groupId];

                EnabledItemInGroup(indexInGroup, groupId, tile, true);
                
                break;
 
        }
        
    }

    private void EnabledItemInGroup(int indexInGroup, int groupId,
        Tile tile, bool isSprite)
    {
        if (indexInGroup < 2)
        {
            if (groupId is 0 or 1)
            {
                tile.isPlayable = true;
                if (isSprite)
                {
                  tile.itemSprite.enabled = true;
                }
                else
                {
                    tile.textMeshPro.enabled = true;
                }
                
                openTileList.Add(tile);
                TileCounter();
            }

        }
    }

    public void RemoveTileOpenTileList(Tile currentTile, Tile targetTile)
    {
        openTileList.Remove(currentTile);
        openTileList.Remove(targetTile);
    }
    public void AddTileOpenTileList(Tile addTile)
    {
        openTileList.Add(addTile);
        
        TileCounter();
    }

    public void CheckDuplicateGroupIds()
    {
        List<int> uniqueIds = new List<int>();
        List<int> similarIds = new List<int>();

        foreach (Tile tile in openTileList)
        {
            int id = tile.groupId;

            if (!uniqueIds.Contains(id))
            {
                uniqueIds.Add(id);
            }
            else
            {
                similarIds.Add(id);
            }
        }

        if (similarIds.Count > 0)
        {
            isGamePlayable = true;
        }
        else
        {
            isGamePlayable = false;
            _uiManager.GameOver();
            OnGameOver?.Invoke();
        }
    }

    private void TileCounter()
    {
        disabledTiles--;

        if (disabledTiles == 0 && isGamePlayable)
        {
            StartCoroutine(MoveAndRemoveTiles());
        }
    }
    
    private IEnumerator MoveAndRemoveTiles()
    {
        while (openTileList.Count > 0)
        {
            Tile currentTile = openTileList[0];
            int currentId = currentTile.groupId;

            List<Tile> matchingTiles = openTileList.FindAll(tile => tile.groupId == currentId);

            foreach (var matchingTile in matchingTiles)
            {
                if (matchingTile != currentTile)
                {
                    Vector3 targetPosition = matchingTile.transform.position;
                    currentTile.transform.DOMove(targetPosition, .2f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        openTileList.Remove(matchingTile);
                        openTileList.Remove(currentTile);
                        
                        starParticle.transform.position = matchingTile.transform.position;
                        starParticle.Play();
                        
                        matchingTile.gameObject.SetActive(false);
                        currentTile.gameObject.SetActive(false);
                    });

                    yield return new WaitForSeconds(.25f);
                    break; 
                }
            }
        }
        _uiManager.LevelCompleted();
        print("level completed");
    }
  
}

