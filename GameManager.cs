using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        GameObject unit;
        Unit unitScript;

        unit = Instantiate(penguinViking, new Vector3(0, 0, 0), transform.rotation);
        unitScript = unit.GetComponent<Unit>();
        unitScript.initStats(unitID, 1, 1, 3, 2, 1);
        AddUnit(unit);
        unitID++;

        unit = Instantiate(penguinViking, new Vector3(-0.5f, 0.75f, 0), transform.rotation);
        unitScript = unit.GetComponent<Unit>();
        unitScript.initStats(unitID, 1, 1, 1, 1, 1);
        AddUnit(unit);
        unitID++;

        unit = Instantiate(penguinViking, new Vector3(0, -1.5f, 0), transform.rotation);
        unitScript = unit.GetComponent<Unit>();
        unitScript.initStats(unitID, 1, 1, 2, 3, 1);
        AddUnit(unit);
        unitID++;
    }

    public void Turn()
    {
        Unit unitScript = units[turn].GetComponent<Unit>();
        unitScript.PrintID();

        

        // focus on and display movement range of unit
        unitScript.Move();

        

        
    }

    public void EndTurn()
    {
        mapManager.ClearHLTiles();

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
        Debug.Log("Unit list count: " + units.Count);

        // order units based on initiative (NOT WORKED ON YET)
        // units.Sort()
        // units = units.OrderBy(e => e.GetComponent<Unit>().GetInit()).ToList();
        units = units.OrderByDescending(e => e.GetComponent<Unit>().GetInit()).ToList();

        turn = 0;
    }

    //private int SortByInit(Unit unit1, Unit unit2)
    //{
    //    if(unit1.GetInit() >= unit2.GetInit())
    //    {
    //        return unit1;
    //    }
    //}

}
