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

    private float atk;
    private float def;
    private float init;
    private float spd; // determines movement range
    private float hp;

    private MapManager mapManager;
    private GameManager gameManager;

    //private Tilemap moveHL;

    void Start()
    {
        mapManager = GameObject.Find("Map Manager").GetComponent<MapManager>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        Tilemap tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        //moveHL = GameObject.Find("Move Indicator").GetComponent<Tilemap>();

        location = tilemap.WorldToCell(transform.position);
        // Debug.Log("Unit location: " + location);

        // set occupancy state at unit location in mapManager to true
        mapManager.SetOccupancy(true, location);

        // add the unit to gameManager
        gameManager.AddUnit(gameObject);
        // Debug.Log("Unit count: " + gameManager.units.Count);
    }

    void Update()
    {
        
    }

    public void initStats(int id_val, int atk_val = 0, int def_val = 0, int init_val = 0, int spd_val = 0, int hp_val = 0)
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
    }
}
