using System.Collections;
using UnityEngine;
using TMPro;
public class Tile : MonoBehaviour
{
    public SpriteRenderer itemSprite;
    public TextMeshPro textMeshPro;
    [Space(5)]
    public int groupId;
    [Space(5)]
    public int x;
    public int y;
    [Space(5)]
    public bool isPlayable;

    private SpriteRenderer _spriteRenderer;
    private void Start()
    {
        GameManager.OnGameOver += GameEnd;

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDestroy()
    {
        GameManager.OnGameOver -= GameEnd;
    }

    public void OpenSurroundingTiles()
    {
        Vector2 currentPos = transform.position;

        Vector2[] directions = {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right,
            new Vector2(-1, 1), new Vector2(1, 1), new Vector2(-1, -1), new Vector2(1, -1)
        };

        foreach (Vector2 dir in directions)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPos + dir, Vector2.zero);
          
            if (hit.collider != null && hit.collider.CompareTag("Tile"))
            {
                Tile neighborTile = hit.collider.GetComponent<Tile>();
               
                if (neighborTile != null && !neighborTile.isPlayable)
                {
                    neighborTile.isPlayable = true;

                    switch (GameManager.instance.itemType)
                    {
                        case ItemType.Sprite:
                            neighborTile.itemSprite.enabled = true;
                            break;
                        case ItemType.Text:
                            neighborTile.textMeshPro.enabled = true;
                            break;
                        
                        case ItemType.Color:
                            neighborTile.itemSprite.enabled = true;
                            break;
                    }
                    
                    GameManager.instance.AddTileOpenTileList(neighborTile);

                    neighborTile.TileColorEffect();

                }
            }
        }
        
        GameManager.instance.CheckDuplicateGroupIds();
    }
    
    public void TileColorEffect()
    {
        StartCoroutine(HighlightColorTile());
    }
    private IEnumerator HighlightColorTile()
    {
        _spriteRenderer.color = Color.yellow;
       
        yield return new WaitForSeconds(0.15f);
        
        _spriteRenderer.color = Color.white;
        
    }

    private void GameEnd()
    {
        isPlayable = false;
    }
}
