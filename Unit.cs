//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    private Vector3 location; //get closest tile coordinate, or just set default to tilemap origin
    private float speed; // determines movement range
    private MapManager mapManager;

    void Start()
    {
        mapManager = GameObject.Find("Map Manager").GetComponent<MapManager>();
        Tilemap tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        location = tilemap.WorldToCell(transform.position);
        Debug.Log(location);

        // set occupancy state at unit location in mapManager to true
        mapManager.SetOccupancy(true, location);
    }

    void Update()
    {
        
    }
}
