// handles game turns, tracks game units and UI,

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

    private GameObject startButton;
    private GameObject endButton;

    private GameObject currentUnit;

    void Start()
    {
        mapManager = GameObject.Find("Map Manager").GetComponent<MapManager>();

        startButton = GameObject.Find("Start Button");
        endButton = GameObject.Find("ET Button");
        startButton.SetActive(false);
        endButton.SetActive(false);
    }

    public void SpawnSampleUnits()
    {
        GameObject unit;
        UnitGeneral generalUnit;
        UnitMovement movementUnit;
        UnitAttack attackUnit;

        unit = Instantiate(penguinViking, new Vector3(0, 0, 0), transform.rotation);
        generalUnit = unit.GetComponent<UnitGeneral>();
        generalUnit.initGenStats(unitID, 0, 2, 10);
        movementUnit = unit.GetComponent<UnitMovement>();
        movementUnit.SetUnitSpd(2);
        attackUnit = unit.GetComponent<UnitAttack>();
        attackUnit.setAtkStats(10, 1, 10);
        AddUnit(unit);
        unitID++;

        unit = Instantiate(penguinViking, new Vector3(3f, 0f, 0), transform.rotation);
        generalUnit = unit.GetComponent<UnitGeneral>();
        generalUnit.initGenStats(unitID, 0, 2, 10);
        movementUnit = unit.GetComponent<UnitMovement>();
        movementUnit.SetUnitSpd(2);
        attackUnit = unit.GetComponent<UnitAttack>();
        attackUnit.setAtkStats(10, 1, 10);
        AddUnit(unit);
        unitID++;

        unit = Instantiate(penguinViking, new Vector3(0, -3.5f, 0), transform.rotation);
        generalUnit = unit.GetComponent<UnitGeneral>();
        generalUnit.initGenStats(unitID, 1, 3, 10);
        movementUnit = unit.GetComponent<UnitMovement>();
        movementUnit.SetUnitSpd(3);
        attackUnit = unit.GetComponent<UnitAttack>();
        attackUnit.setAtkStats(10, 1, 10);
        AddUnit(unit);
        unitID++;
    }

    public void AddUnit(GameObject unit) // add the unit to the unit tracking list
    {
        units.Add(unit);
    }

    // generate the units in battle and setup turns
    public void InitializeBattle()
    {
        SpawnSampleUnits();
        Debug.Log("Unit count: " + units.Count);

        // order units on list based on init stat
        units = units.OrderByDescending(e => e.GetComponent<UnitGeneral>().GetInit()).ToList();

        turn = 0;

        Destroy(GameObject.Find("Init Button")); 
        startButton.SetActive(true);
    }

    // Allow the current unit to perform actions on its turn
    public void PlayTurn()
    {
        currentUnit = units[turn]; // keep track of the current turn's unit

        UnitGeneral generalUnit = units[turn].GetComponent<UnitGeneral>();
        UnitMovement movementUnit = units[turn].GetComponent<UnitMovement>();
        UnitAttack attackUnit = units[turn].GetComponent<UnitAttack>();

        Debug.Log("Unit: " + generalUnit.GetID() + "'s turn");

        // focus camera on unit
        generalUnit.Focus();

        // allow movement
        movementUnit.Move();
        attackUnit.ShowAttacks();

        startButton.SetActive(false);
        endButton.SetActive(true);
    }

    // ends the current turn to move on to the next unit
    public void EndTurn()
    {
        mapManager.ClearHLTiles();

        // disallow movement of the unit before moving on
        UnitMovement movementUnit = units[turn].GetComponent<UnitMovement>();
        movementUnit.setMovable(false);

        // go to the next unit's turn
        if (turn == units.Count - 1)
        {
            turn = 0;
        }
        else
        {
            turn++;
        }

        PlayTurn();
    }

    // return the unit occupying the location, if not occupied, print warning and return null
    public GameObject GetUnit(Vector3Int location)
    {
        // go through each unit to find whether any unit shares the same location
        for(int i = 0; i < units.Count; i++)
        {
            UnitMovement movementUnit = units[i].GetComponent<UnitMovement>();
            if(movementUnit.getLoc() == location)
            {
                return units[i];
            }
        }

        return null;
    }
}
