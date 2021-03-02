// manages tile states and effects

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private Tilemap tilemap;
    private Tilemap moveHL; // tilemap to highlight movement range
    private Tilemap attackHL; // tilemap to highlight attack range

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
    [SerializeField] private List<bool> tileMovable;
    [SerializeField] private List<bool> tileAtkable;

    [SerializeField] TileBase moveHLTile;
    [SerializeField] TileBase atkHLTile;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

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
            tileMovable.Add(false);
            tileAtkable.Add(false);

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
        //if ((tileCoordinate.x >= bounds.x) && (tileCoordinate.x < (bounds.x + bounds.size.x)) && (tileCoordinate.y >= bounds.y) && (tileCoordinate.y < (bounds.y + bounds.size.y)))
        //{
        //    //Debug.Log(tileCoordinate);
        //    if (lastTileCoordinate != tileCoordinate && !selected) // check if moved since last time
        //    {
        //        Destroy(oldCursorTile);
        //        Vector3 worldPos = tilemap.CellToWorld(tileCoordinate);
        //        oldCursorTile = Instantiate(cursorTile, worldPos, transform.rotation);
        //    }
        //}

        //lastTileCoordinate = tileCoordinate; // remember last Tile coordinate to check


        if (Input.GetMouseButtonDown(0))
        {
            if ((tileCoordinate.x >= bounds.x) && (tileCoordinate.x < (bounds.x + bounds.size.x)) && (tileCoordinate.y >= bounds.y) && (tileCoordinate.y < (bounds.y + bounds.size.y)))
            {
                // get selected tile information
                selectedTileCoordinate = tileCoordinate;

                TileBase selectedTile = tilemap.GetTile(selectedTileCoordinate);
                // Debug.Log("Selected: " + dataFromTiles[selectedTile].terrain + ", " + selectedTileCoordinate + ", Movable status: " + GetMovableStatus(selectedTileCoordinate) + ", Atkable status: " + GetAtkableStatus(selectedTileCoordinate));
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if ((tileCoordinate.x >= bounds.x) && (tileCoordinate.x < (bounds.x + bounds.size.x)) && (tileCoordinate.y >= bounds.y) && (tileCoordinate.y < (bounds.y + bounds.size.y)))
            {
                // get selected tile information
                selectedTileCoordinate = tileCoordinate;

                TileBase selectedTile = tilemap.GetTile(selectedTileCoordinate);
                Debug.Log("Selected: " + dataFromTiles[selectedTile].terrain + ", " + selectedTileCoordinate + ", Movable status: " + GetMovableStatus(selectedTileCoordinate) + ", Atkable status: " + GetAtkableStatus(selectedTileCoordinate));
            }
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    if ((tileCoordinate.x >= bounds.x) && (tileCoordinate.x < (bounds.x + bounds.size.x)) && (tileCoordinate.y >= bounds.y) && (tileCoordinate.y < (bounds.y + bounds.size.y)))
        //    {
        //        // keep selection display at mouse location when clicked, right click to deselect
        //        if (!selected)
        //        {
        //            selected = true;

        //            // get selected tile information
        //            selectedTileCoordinate = tileCoordinate;

        //            TileBase selectedTile = tilemap.GetTile(selectedTileCoordinate);
        //            Debug.Log("Selected: " + dataFromTiles[selectedTile].terrain + ", " + selectedTileCoordinate + ", Movable status: " + GetMovableStatus(selectedTileCoordinate));
        //        }
        //    }
        //}

        //if (Input.GetMouseButtonDown(1))
        //{
        //    if (selected)
        //    {
        //        selected = false;
        //        // Debug.Log("Deselected");
        //    }
        //}
    }

    // returns moveable status
    public bool GetMovableStatus(Vector3Int location)
    {
        for (int i = 0; i < tileCoordinates.Count; i++)
        {
            if (tileCoordinates[i] == location)
            {
                return tileMovable[i]; ;
            }
        }

        return false;
    }

    public bool GetAtkableStatus(Vector3Int location)
    {
        for (int i = 0; i < tileCoordinates.Count; i++)
        {
            if (tileCoordinates[i] == location)
            {
                return tileAtkable[i]; ;
            }
        }

        return false;
    }

    public bool GetOccupancyStatus(Vector3Int location)
    {
        for (int i = 0; i < tileCoordinates.Count; i++)
        {
            if (tileCoordinates[i] == location)
            {
                return tileOccupancy[i]; ;
            }
        }

        return false;
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

    private void SetMovable(bool val, Vector3Int location)
    {
        for (int i = 0; i < tileCoordinates.Count; i++)
        {
            if (tileCoordinates[i] == location)
            {
                tileMovable[i] = val;
            }
        }
    }

    private void SetAtkable(bool val, Vector3Int location)
    {
        for (int i = 0; i < tileCoordinates.Count; i++)
        {
            if (tileCoordinates[i] == location)
            {
                tileAtkable[i] = val;
            }
        }
    }

    // given the unit location and its spd stat, compute and display movable hexes, also marks those hexes as movable
    public void CheckMovement(Vector3 unitLoc, float unitSpd)
    {
        // Debug.Log("UnitLoc: " + unitLoc); // cell position

        float x = unitLoc.x;
        float y = unitLoc.y;

        // moveHL.SetTile(new Vector3Int((int)x, (int)y, 0), moveHLTile);

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
                SetMovable(true, new Vector3Int((int)x, (int)y, 0));
            }

            for (int j = 0; j < i; j++)
            {
                // go left
                x--;
                SetMovable(true, new Vector3Int((int)x, (int)y, 0));
            }

            for (int j = 0; j < i; j++)
            {
                // go down left
                if (Mathf.Abs(y) % 2 == 0) // current y is even 
                {
                    x--;
                }
                y--;
                SetMovable(true, new Vector3Int((int)x, (int)y, 0));
            }

            for (int j = 0; j < i; j++)
            {
                // go down right
                if (Mathf.Abs(y) % 2 == 1) // current y is odd 
                {
                    x++;
                }
                y--;
                SetMovable(true, new Vector3Int((int)x, (int)y, 0));
            }

            for (int j = 0; j < i; j++)
            {
                // go right
                x++;
                SetMovable(true, new Vector3Int((int)x, (int)y, 0));
            }

            for (int j = 0; j < i; j++)
            {
                // go up right
                if (Mathf.Abs(y) % 2 == 1) // current y is odd 
                {
                    x++;
                }
                y++;
                SetMovable(true, new Vector3Int((int)x, (int)y, 0));
            }
        }

        // if any hex is occupied, make it not movable
        for (int i = 0; i < tileCoordinates.Count; i++)
        {
            if (tileOccupancy[i] == true)
            {
                tileMovable[i] = false;
            }
        }

        // highlight movable tiles
        for (int i = 0; i < tileCoordinates.Count; i++)
        {
            if (tileMovable[i] == true)
            {
                moveHL.SetTile(tileCoordinates[i], moveHLTile);
            }
        }
    }

    public void ClearHLTiles()
    {
        moveHL.ClearAllTiles();

        // reset all movable and atkable status to false
        for (int i = 0; i < tileMovable.Count; i++)
        {
            tileMovable[i] = false;
        }

        for (int i = 0; i < tileAtkable.Count; i++)
        {
            tileAtkable[i] = false;
        }
    }

    public void CheckAttack(Vector3 unitLoc, float unitRng, int unitStatus)
    {
        // Debug.Log("UnitLoc: " + unitLoc); // cell position

        //top right (+1 to y from unitLoc, also +1 to x if going from odd y to even y)
        float x = unitLoc.x;
        float y = unitLoc.y;

        // moveHL.SetTile(new Vector3Int((int)x, (int)y, 0), moveHLTile);

        // go around the unitLoc to check if any enemies are in range, if enemy found, get its information and make that hex clickable for attack
        for (int i = 1; i <= unitRng; i++)
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

                // iterate through unit list to check if any of them are on the hex
                for (int k = 0; k < gameManager.units.Count; k++)
                {
                    if(gameManager.units[k] != null)
                    {
                        UnitMovement movementUnit = gameManager.units[k].GetComponent<UnitMovement>();
                        UnitGeneral generalUnit = gameManager.units[k].GetComponent<UnitGeneral>();
                        if ((new Vector3Int((int)x, (int)y, 0) == movementUnit.GetLoc()) && (generalUnit.GetStatus() != unitStatus))
                        {
                            SetAtkable(true, new Vector3Int((int)x, (int)y, 0));
                        }
                    }
                }
            }

            for (int j = 0; j < i; j++)
            {
                // go left
                x--;

                for (int k = 0; k < gameManager.units.Count; k++)
                {
                    if (gameManager.units[k] != null)
                    {
                        UnitMovement movementUnit = gameManager.units[k].GetComponent<UnitMovement>();
                        UnitGeneral generalUnit = gameManager.units[k].GetComponent<UnitGeneral>();
                        if ((new Vector3Int((int)x, (int)y, 0) == movementUnit.GetLoc()) && (generalUnit.GetStatus() != unitStatus))
                        {
                            SetAtkable(true, new Vector3Int((int)x, (int)y, 0));
                        }
                    }
                }
            }

            for (int j = 0; j < i; j++)
            {
                // go down left
                if (Mathf.Abs(y) % 2 == 0) // current y is even 
                {
                    x--;
                }
                y--;

                for (int k = 0; k < gameManager.units.Count; k++)
                {
                    if (gameManager.units[k] != null)
                    {
                        UnitMovement movementUnit = gameManager.units[k].GetComponent<UnitMovement>();
                        UnitGeneral generalUnit = gameManager.units[k].GetComponent<UnitGeneral>();
                        if ((new Vector3Int((int)x, (int)y, 0) == movementUnit.GetLoc()) && (generalUnit.GetStatus() != unitStatus))
                        {
                            SetAtkable(true, new Vector3Int((int)x, (int)y, 0));
                        }
                    }
                }
            }

            for (int j = 0; j < i; j++)
            {
                // go down right
                if (Mathf.Abs(y) % 2 == 1) // current y is odd 
                {
                    x++;
                }
                y--;

                for (int k = 0; k < gameManager.units.Count; k++)
                {
                    if (gameManager.units[k] != null)
                    {
                        UnitMovement movementUnit = gameManager.units[k].GetComponent<UnitMovement>();
                        UnitGeneral generalUnit = gameManager.units[k].GetComponent<UnitGeneral>();
                        if ((new Vector3Int((int)x, (int)y, 0) == movementUnit.GetLoc()) && (generalUnit.GetStatus() != unitStatus))
                        {
                            SetAtkable(true, new Vector3Int((int)x, (int)y, 0));
                        }
                    }
                }
            }

            for (int j = 0; j < i; j++)
            {
                // go right
                x++;

                for (int k = 0; k < gameManager.units.Count; k++)
                {
                    if (gameManager.units[k] != null)
                    {
                        UnitMovement movementUnit = gameManager.units[k].GetComponent<UnitMovement>();
                        UnitGeneral generalUnit = gameManager.units[k].GetComponent<UnitGeneral>();
                        if ((new Vector3Int((int)x, (int)y, 0) == movementUnit.GetLoc()) && (generalUnit.GetStatus() != unitStatus))
                        {
                            SetAtkable(true, new Vector3Int((int)x, (int)y, 0));
                        }
                    }
                }
            }

            for (int j = 0; j < i; j++)
            {
                // go up right
                if (Mathf.Abs(y) % 2 == 1) // current y is odd 
                {
                    x++;
                }
                y++;

                for (int k = 0; k < gameManager.units.Count; k++)
                {
                    if (gameManager.units[k] != null)
                    {
                        UnitMovement movementUnit = gameManager.units[k].GetComponent<UnitMovement>();
                        UnitGeneral generalUnit = gameManager.units[k].GetComponent<UnitGeneral>();
                        if ((new Vector3Int((int)x, (int)y, 0) == movementUnit.GetLoc()) && (generalUnit.GetStatus() != unitStatus))
                        {
                            SetAtkable(true, new Vector3Int((int)x, (int)y, 0));
                        }
                    }
                }
            }
        }

        for (int i = 0; i < tileCoordinates.Count; i++)
        {
            if (tileAtkable[i] == true)
            {
                moveHL.SetTile(tileCoordinates[i], atkHLTile);
            }
        }
    }
}
