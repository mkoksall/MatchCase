using UnityEngine;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    public static Board instance;
    private int _width;
    private int _height;
    public int tileSize=1;
    public GameObject tilePrefab;
    private Tile[,] _allTiles;
    private int _groupId = 0;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _width = GameManager.instance.width;
        _height = GameManager.instance.height;
        
        
        _allTiles = new Tile[_width, _height];
        
        GenerateGrid();

        GenerateGroups();
    }

    private void GenerateGrid()
    {
        for (int i = 0; i < _height; i ++)
        {
            for (int j = 0; j < _width; j ++){
         
                Vector2 tempPosition = new Vector2(j, i );
                GameObject tile = Instantiate(tilePrefab, tempPosition,Quaternion.identity);
             
                tile.transform.parent = this.transform;
                tile.name = "( " + (i+1) + ", " + (j+1) + " )";

                Tile tileScript = tile.GetComponent<Tile>();
                
                tileScript.x = i;
                tileScript.y = j;
                tileScript.groupId = -1; 
                
                _allTiles[j, i] = tile.GetComponent<Tile>();
                
            }

        }
        float gridW = _width * tileSize;
        float gridH = _height * tileSize;

        transform.position = new Vector2((-gridW / 2+ tileSize*0.5f ), (-gridH / 2+ tileSize*0.5f));
     
    }

    public void MoveTilesDown(int x, int y)
    {
        for (int i = y + 1; i < _height; i++)
        {
            if (_allTiles[x, i] != null)
            {
                _allTiles[x, i].transform.position += Vector3.down * tileSize;
                _allTiles[x, i].GetComponent<DragAndDrop>().firstPosition = _allTiles[x, i].transform.position;
     
            }
        }
    }
    
    private void GenerateGroups()
    {
        int numGroups4 = (_width*_height)/4; 
        int numGroups2 = Mathf.CeilToInt((_width * _height - numGroups4 * 4) / 2f); 

        for (int i = 0; i < numGroups4; i++)
        {
            GenerateGroup(4);
        }

        for (int i = 0; i < numGroups2; i++)
        {
            GenerateGroup(2);
        }
    }

    private void GenerateGroup(int groupSize)
    {
        for (int i = 0; i < groupSize; i++)
        {
            Vector2 position = GetRandomEmptyPosition();
            Tile tile = _allTiles[(int)position.x, (int)position.y];

            tile.groupId = _groupId;
            
            GameManager.instance.SetItemToTile(tile,_groupId,i);

        }

        _groupId++;
    }

    private Vector2 GetRandomEmptyPosition()
    {
        Vector2 randomPosition = new Vector2(Random.Range(0, _width), Random.Range(0, _height));
        
        while (_allTiles[(int)randomPosition.x, (int)randomPosition.y].groupId != -1)
        {
            randomPosition = new Vector2(Random.Range(0, _width), Random.Range(0, _height));
        }

        return randomPosition;
    }
    
}
