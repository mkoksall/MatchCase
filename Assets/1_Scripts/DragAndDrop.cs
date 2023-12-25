using System;
using System.Collections;
using UnityEngine;
public class DragAndDrop : MonoBehaviour
{
    [HideInInspector]
    public Vector2 firstPosition;
   
    private Tile _tile;
    private SpriteRenderer _spriteRenderer;

    public static event Action<Transform> OnPlayParticle;
    private void Start()
    {
        firstPosition = transform.position;
        _tile = GetComponent<Tile>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if(!_tile.isPlayable) return;
        
        StartCoroutine(DragTile());
        _spriteRenderer.sortingOrder = 2;
        _tile.itemSprite.sortingOrder = 3;
        _tile.textMeshPro.sortingOrder = 3;
    }

    private IEnumerator DragTile()
    {
        while (Input.GetMouseButton(0))
        {
            Vector3 newPosition = (Vector3)Camera.main.ScreenToWorldPoint(Input.mousePosition) ;
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
            yield return null;
        }

        CheckMatch();
    }

    private void CheckMatch()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f);

        foreach (Collider2D targetTileCollider in colliders)
        {
            if (targetTileCollider.gameObject != gameObject && targetTileCollider.GetComponent<Tile>() != null)
            {
                Tile currentTile = gameObject.GetComponent<Tile>();
                Tile targetTile = targetTileCollider.GetComponent<Tile>();

                if (targetTile.isPlayable)
                {
                    if (targetTile.groupId == currentTile.groupId)
                    {
                        OnPlayParticle?.Invoke(transform);
                      
                        gameObject.SetActive(false);
                        targetTile.gameObject.SetActive(false);
                      
                        GameManager.instance.RemoveTileOpenTileList(currentTile,targetTile);
                        
                        targetTile.OpenSurroundingTiles();
                        
                        Board.instance.MoveTilesDown(targetTile.y,targetTile.x);
                        Board.instance.MoveTilesDown(currentTile.y,currentTile.x);
                        break;
                    }
                }
                
            }
        }

        transform.position = firstPosition;
        _spriteRenderer.sortingOrder = 0;
        _tile.itemSprite.sortingOrder = 1;
        _tile.textMeshPro.sortingOrder = 1;
    }
    
}
