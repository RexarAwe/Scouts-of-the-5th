using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject penguinViking;

    public int turn;
    [SerializeField] public List<GameObject> units;
    private int unitID = 0;

    private MapManager mapManager;

    void Start()
    {
        mapManager = GameObject.Find("Map Manager").GetComponent<MapManager>();
    }

    void Update()
    {
        
    }

    public void SpawnSampleUnits()
    {
        Unit unitScript;
        unitScript = Instantiate(penguinViking, new Vector3(0, 0, 0), transform.rotation).GetComponent<Unit>();
        unitScript.initStats(unitID, 1, 1, 1, 2, 1);
        unitID++;
        unitScript = Instantiate(penguinViking, new Vector3(-0.5f, 0.75f, 0), transform.rotation).GetComponent<Unit>();
        unitScript.initStats(unitID, 1, 1, 1, 1, 1);
        unitID++;
        unitScript = Instantiate(penguinViking, new Vector3(0, -1.5f, 0), transform.rotation).GetComponent<Unit>();
        unitScript.initStats(unitID, 1, 1, 1, 3, 1);
        unitID++;
    }

    public void Turn()
    {
        Unit unitScript = units[turn].GetComponent<Unit>();
        unitScript.PrintID();

        mapManager.ClearHLTiles();

        // focus on and display movement range of unit
        unitScript.Move();
        

        if (turn == units.Count - 1) // reset turns
        {
            turn = 0;

        }
        else
        {
            turn++;
        }
    }

    public void AddUnit(GameObject unit)
    {
        units.Add(unit);
    }

    public void StartBattle()
    {
        SpawnSampleUnits();

        // order units based on initiative (NOT WORKED ON YET)

        turn = 0;
    }
}
