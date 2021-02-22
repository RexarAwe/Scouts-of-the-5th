//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    private Vector3Int location; //get closest tile coordinate, or just set default to tilemap origin

    private int id;
    private int status; // friend or enemy

    private int atk;
    private int def;
    private int init;
    private int spd; // determines movement range
    private int hp;
    private int rng; // determines attack range

    private MapManager mapManager;
    private GameManager gameManager;

    Tilemap tilemap;

    private bool movable = false;

    //private Tilemap moveHL;

    void Start()
    {
        mapManager = GameObject.Find("Map Manager").GetComponent<MapManager>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        //moveHL = GameObject.Find("Move Indicator").GetComponent<Tilemap>();

        location = tilemap.WorldToCell(transform.position);
        // Debug.Log("Unit location: " + location);

        // set occupancy state at unit location in mapManager to true
        mapManager.SetOccupancy(true, location);

        // add the unit to gameManager
        // gameManager.AddUnit(gameObject);
        // Debug.Log("Unit count: " + gameManager.units.Count);
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(movable)
            {
                Debug.Log("move here");
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

                //Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //Vector3Int tileCoordinate = tilemap.WorldToCell(mouseWorldPos);

                
            }
        }

        //if (movable)
        //{
        //    Debug.Log("move here 1");
        //    if (Input.GetMouseButtonDown(1))
        //    {
        //        Debug.Log("move here");
        //    }

        //    movable = false;
        //}
    }

    public void setMovable(bool val)
    {
        movable = val;
    }

    public void initStats(int id_val, int atk_val = 0, int def_val = 0, int init_val = 0, int spd_val = 0, int hp_val = 0, int rng_val = 0)
    {
        id = id_val;
        atk = atk_val;
        def = def_val;
        init = init_val;
        spd = spd_val;
        hp = hp_val;
    }

    public void PrintID()
    {
        Debug.Log("Unit: " + id + "'s turn");
    }

    public void Move()
    {
        // focus camera
        Camera.main.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Camera.main.transform.position.z);

        // "highlight" movement range
        mapManager.CheckMovement(location, spd);

        // onclick, moves that unit to that hex
        // if click on any tile that has movable == true, move unit location there
        movable = true;
    }

    public float GetInit()
    {
        return init;
    }
}
