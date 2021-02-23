// handles unit movement based on spd stat as well as track location

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitMovement : MonoBehaviour
{
    protected Vector3Int location; //get closest tile coordinate, or just set default to tilemap origin
    protected int spd; // determines movement range
    protected bool movable = false;

    [SerializeField] protected MapManager mapManager;
    protected GameManager gameManager;

    Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        mapManager = GameObject.Find("Map Manager").GetComponent<MapManager>();
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();

        location = tilemap.WorldToCell(transform.position);

        // set occupancy state at unit location in mapManager to true
        mapManager.SetOccupancy(true, location);
    }

    protected void Update()
    {
        // onclick, if tile is movable, moves unit there and updates location
        if (Input.GetMouseButtonDown(0))
        {
            if (movable)
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int tileCoordinate = tilemap.WorldToCell(mouseWorldPos);

                if (mapManager.GetMovableStatus(tileCoordinate))
                {
                    mapManager.SetOccupancy(false, location);

                    // move the unit there
                    location = tileCoordinate;
                    transform.position = tilemap.CellToWorld(location);

                    // change occupancy of tiles
                    mapManager.SetOccupancy(true, location);

                    movable = false;
                }
            }
        }
    }

    // focuses camera on the current turn's unit
    public void Move()
    {
        // "highlight" movement range
        mapManager.CheckMovement(location, spd);

        // allows movement for this unit
        movable = true;
    }

    // movement
    public void setMovable(bool val)
    {
        movable = val;
    }

    public void SetUnitSpd(int val)
    {
        spd = val;
    }
}
