// manages tile states and effects

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private Tilemap tilemap;
    private Tilemap moveHL; // to highlight movement range

    private Vector3 mouseWorldPos;
    private Vector3Int tileCoordinate;
    private Vector3Int lastTileCoordinate;
    private BoundsInt bounds;

    // to display selection
    private bool selected;
    private Vector3Int selectedTileCoordinate;
    [SerializeField] private GameObject cursorTile;
    private GameObject oldCursorTile;

    // tileData provides more info about the TileBase
    [SerializeField] private List<TileData> tileDatas;
    private Dictionary<TileBase, TileData> dataFromTiles;

    // store tilemap information needed for gameplay
    [SerializeField] private List<Vector3Int> tileCoordinates;
    [SerializeField] private List<bool> tileOccupancy;

    [SerializeField] TileBase moveHLTile;

    void Start()
    {
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        moveHL = GameObject.Find("Move Indicator").GetComponent<Tilemap>();

        tilemap.CompressBounds();
        bounds = tilemap.cellBounds;

        // setup all tiles information
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds); // arranged left to right, bottom to top

        // populate list of all tile coordinates, left to right, bottom to top
        int x = tilemap.origin.x;
        int y = tilemap.origin.y;
        int z = tilemap.origin.z;

        for (int i = 0; i < allTiles.Length; i++)
        {
            tileCoordinates.Add(new Vector3Int(x, y, z));
            tileOccupancy.Add(false);

            if (x < tilemap.origin.x + bounds.size.x - 1)
            {
                x++;
            }
            else
            {
                x = tilemap.origin.x;
                y++;
            }
        }

        // automatically pair tilebase asset to tiledata
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (TileData tileData in tileDatas)
        {
            foreach (TileBase tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }

        // Debug.Log(allTiles.Length);
        // Debug.Log(bounds.size.x + ", " + bounds.size.y);
        // Debug.Log("Tilemap origin: " + tilemap.origin);
    }

    void Update()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tileCoordinate = tilemap.WorldToCell(mouseWorldPos);

        // make sure within tilebox bounds, then create selection display at mouse location
        if ((tileCoordinate.x >= bounds.x) && (tileCoordinate.x < (bounds.x + bounds.size.x)) && (tileCoordinate.y >= bounds.y) && (tileCoordinate.y < (bounds.y + bounds.size.y)))
        {
            //Debug.Log(tileCoordinate);
            if (lastTileCoordinate != tileCoordinate && !selected) // check if moved since last time
            {
                Destroy(oldCursorTile);
                Vector3 worldPos = tilemap.CellToWorld(tileCoordinate);
                oldCursorTile = Instantiate(cursorTile, worldPos, transform.rotation);
            }
        }

        lastTileCoordinate = tileCoordinate; // remember last Tile coordinate to check

        
        if (Input.GetMouseButtonDown(0))
        {
            if ((tileCoordinate.x >= bounds.x) && (tileCoordinate.x < (bounds.x + bounds.size.x)) && (tileCoordinate.y >= bounds.y) && (tileCoordinate.y < (bounds.y + bounds.size.y)))
            {
                // keep selection display at mouse location when clicked, right click to deselect
                if (!selected)
                {
                    selected = true;

                    // get selected tile information
                    selectedTileCoordinate = tileCoordinate;

                    TileBase selectedTile = tilemap.GetTile(selectedTileCoordinate);
                    Debug.Log("Selected: " + dataFromTiles[selectedTile].terrain + ", " + selectedTileCoordinate);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (selected)
            {
                selected = false;
                // Debug.Log("Deselected");
            }
        }
    }

    public void SetOccupancy(bool val, int index)
    {
        tileOccupancy[index] = val;
    }

    public void SetOccupancy(bool val, Vector3Int location) 
    {
        // Debug.Log("location to search: " + location);
        // Debug.Log("tileCoordinates count = " + tileCoordinates.Count);
        for(int i = 0; i < tileCoordinates.Count; i++)
        {
            // Debug.Log("WHAT?");
            // Debug.Log("comparing to index " + i + " coordinates " + tileCoordinates[i]);
            if(tileCoordinates[i] == location)
            {
                // Debug.Log("Setting occupancy at index " + i);
                tileOccupancy[i] = val;
            }
        }
    }

    // given the unit location, compute and display movable hexes
    public void CheckMovement(Vector3 unitLoc, float unitSpd)
    {
        Debug.Log("UnitLoc: " + unitLoc); // cell position

        // moveHL.SetTile(new Vector3Int(0, 0, 0), moveHLTile);

        // based on spd stat, check each direction and settile on Move Indicator tileset, after movement, clear

        // check top right, right, bot right, bot left, left, top left

        //top right (+1 to y from unitLoc, also +1 to x if going from odd y to even y)
        float x = unitLoc.x;
        float y = unitLoc.y;

        moveHL.SetTile(new Vector3Int((int)x, (int)y, 0), moveHLTile);
        
        // go around the unitLoc
        for (int i = 1; i <= unitSpd; i++)
        {
            x = unitLoc.x + i;
            y = unitLoc.y;

            for (int j = 0; j < i; j++)
            {
                // go up left
                if (Mathf.Abs(y) % 2 == 0) // current y is even 
                {
                    x--;
                }
                y++;
                moveHL.SetTile(new Vector3Int((int)x, (int)y, 0), moveHLTile);
            }

            for (int j = 0; j < i; j++)
            {
                // go left
                x--;
                moveHL.SetTile(new Vector3Int((int)x, (int)y, 0), moveHLTile);
            }

            for (int j = 0; j < i; j++)
            {
                // go down left
                if (Mathf.Abs(y) % 2 == 0) // current y is even 
                {
                    x--;
                }
                y--;
                moveHL.SetTile(new Vector3Int((int)x, (int)y, 0), moveHLTile);
            }

            for (int j = 0; j < i; j++)
            {
                // go down right
                if (Mathf.Abs(y) % 2 == 1) // current y is odd 
                {
                    x++;
                }
                y--;
                moveHL.SetTile(new Vector3Int((int)x, (int)y, 0), moveHLTile);
            }

            for (int j = 0; j < i; j++)
            {
                // go right
                x++;
                moveHL.SetTile(new Vector3Int((int)x, (int)y, 0), moveHLTile);
            }

            for (int j = 0; j < i; j++)
            {
                // go up right
                if (Mathf.Abs(y) % 2 == 1) // current y is odd 
                {
                    x++;
                }
                y++;
                moveHL.SetTile(new Vector3Int((int)x, (int)y, 0), moveHLTile);
            }
        }
            
        
    }

    public void ClearHLTiles()
    {
        moveHL.ClearAllTiles();
    }
}
