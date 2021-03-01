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
    private GameObject MoveButton;
    private GameObject AttackButton;

    private GameObject currentUnit;

    // current unit components, for use with buttons
    UnitGeneral generalUnit;
    UnitMovement movementUnit;
    UnitAttack attackUnit;

    void Start()
    {
        mapManager = GameObject.Find("Map Manager").GetComponent<MapManager>();

        startButton = GameObject.Find("Start Button");
        endButton = GameObject.Find("ET Button");
        MoveButton = GameObject.Find("Move Button");
        AttackButton = GameObject.Find("Attack Button");

        startButton.SetActive(false);
        endButton.SetActive(false);
        MoveButton.SetActive(false);
        AttackButton.SetActive(false);
    }

    public void SpawnSampleUnits()
    {
        SpawnRecruit(new Vector3(0, 0, 0), 0);
        SpawnRecruit(new Vector3(3f, 0, 0), 1);
        SpawnRecruit(new Vector3(0, -3.5f, 0), 1);
    }

    private void SpawnRecruit(Vector3 location, int allegiance)
    {
        GameObject unit;
        UnitGeneral generalUnit;
        UnitMovement movementUnit;
        UnitAttack attackUnit;

        unit = Instantiate(penguinViking, location, transform.rotation);
        generalUnit = unit.GetComponent<UnitGeneral>();
        generalUnit.initGenStats(unitID, allegiance, 2, 10);
        movementUnit = unit.GetComponent<UnitMovement>();
        movementUnit.SetUnitSpd(2);
        attackUnit = unit.GetComponent<UnitAttack>();
        attackUnit.SetAtkStats(10, 1, 10);
        attackUnit.SetDmgStats(2, 5, 0);
        AddUnit(unit);
        unitID++;
    }

    public void AddUnit(GameObject unit) // add the unit to the unit tracking list
    {
        units.Add(unit);
    }

    public void RemoveUnit(GameObject unit)
    {
        
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

        currentUnit = units[turn];
    }

    // Allow the current unit to perform actions on its turn
    public void PlayTurn()
    {
        generalUnit = currentUnit.GetComponent<UnitGeneral>();
        movementUnit = currentUnit.GetComponent<UnitMovement>();
        attackUnit = currentUnit.GetComponent<UnitAttack>();

        generalUnit.SetActions(2); // set the unit's action capacity for the turn

        Debug.Log("Unit: " + generalUnit.GetID() + "'s turn, HP: " + generalUnit.GetHP());

        // focus camera on unit
        generalUnit.Focus();

        // allow for actions
        TakeAction();
        //movementUnit.Move();
        //attackUnit.Attack();

        startButton.SetActive(false);
        endButton.SetActive(true);
    }

    // allow unit to take an action and adjust remaining actions
    public void TakeAction()
    {
        MoveButton.SetActive(true);
        AttackButton.SetActive(true);

        // adjust action points
    }

    public void MoveAction()
    {
        movementUnit.Move();
    }

    public void AttackAction()
    {
        attackUnit.Attack();
    }

    // ends the current turn to move on to the next unit
    public void EndTurn()
    {
        MoveButton.SetActive(false);
        AttackButton.SetActive(false);

        mapManager.ClearHLTiles();

        // disallow movement of the unit before moving on
        UnitMovement movementUnit = currentUnit.GetComponent<UnitMovement>();
        movementUnit.SetMovable(false);

        // remove any dead units
        units = units.Where(unit => unit != null).ToList();

        // go to the next unit's turn
        if (turn >= units.Count - 1)
        {
            turn = 0;
        }
        else
        {
            turn++;
        }

        currentUnit = units[turn]; // keep track of the current turn's unit

        PlayTurn();
    }

    // return the unit occupying the location, if not occupied, print warning and return null
    public GameObject GetUnit(Vector3Int location)
    {
        // go through each unit to find whether any unit shares the same location
        for(int i = 0; i < units.Count; i++)
        {
            UnitMovement movementUnit = units[i].GetComponent<UnitMovement>();
            if(movementUnit.GetLoc() == location)
            {
                return units[i];
            }
        }

        return null;
    }
}
