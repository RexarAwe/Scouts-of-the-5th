using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    private Tilemap tilemap;
    private Vector3 mouseWorldPos;
    private Vector3Int tileCoordinate;
    private Vector3Int lastTileCoordinate;

    private GameObject oldCursorTile;

    private BoundsInt bounds;

    private bool selected;
    private Vector3Int selectedTileCoordinate;

    //private TileBase tile2;

    [SerializeField] private GameObject cursorTile;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        tilemap.CompressBounds();
        bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        //Debug.Log(allTiles.Length);
        //Debug.Log(bounds.size.x + ", " + bounds.size.y);

        //for (int x = 0; x < bounds.size.x; x++)
        //{
        //    for (int y = 0; y < bounds.size.y; y++)
        //    {
        //        TileBase tile = allTiles[x + y * bounds.size.x];
        //        if (tile != null)
        //        {
        //            Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
        //        }
        //        else
        //        {
        //            Debug.Log("x:" + x + " y:" + y + " tile: (null)");
        //        }
        //    }
        //}

        //tile2 = allTiles[3];
        //tilemap.SetTile(new Vector3Int(-1, 0, 0), tile2); // uses transform positions

        //Debug.Log(tilemap.origin);
    }

    void Update()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tileCoordinate = tilemap.WorldToCell(mouseWorldPos);

        // make sure within tilebox bounds
        if ((tileCoordinate.x >= bounds.x) && (tileCoordinate.x < (bounds.x + bounds.size.x)) && (tileCoordinate.y >= bounds.y) && (tileCoordinate.y < (bounds.y + bounds.size.y)))
        {
            //Debug.Log(tileCoordinate);
            if (lastTileCoordinate != tileCoordinate && !selected) // check if moved since last time
            {
                Destroy(oldCursorTile);
                Vector3 worldPos = tilemap.CellToWorld(tileCoordinate);
                oldCursorTile = Instantiate(cursorTile, worldPos, transform.rotation);
            }
            //tilemap.SetTile(tileCoordinate, tile2); // uses transform positions?

        }

        lastTileCoordinate = tileCoordinate; // remember last Tile coordinate to check

        if (Input.GetMouseButtonDown(0))
        {
            if ((tileCoordinate.x >= bounds.x) && (tileCoordinate.x < (bounds.x + bounds.size.x)) && (tileCoordinate.y >= bounds.y) && (tileCoordinate.y < (bounds.y + bounds.size.y)))
            {
                if(!selected)
                {
                    selected = true;

                    // get selected tile information
                    selectedTileCoordinate = tileCoordinate;
                    Debug.Log("Selected: " + selectedTileCoordinate);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (selected)
            {
                selected = false;
                Debug.Log("Deselected");
            }
        }
    }
}
