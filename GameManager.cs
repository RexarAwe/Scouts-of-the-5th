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

    void Start()
    {
        mapManager = GameObject.Find("Map Manager").GetComponent<MapManager>();

        startButton = GameObject.Find("Button 2");
        startButton.SetActive(false);
        endButton = GameObject.Find("Button 3");
        endButton.SetActive(false);
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

        unit = Instantiate(penguinViking, new Vector3(3f, 0f, 0), transform.rotation);
        unitScript = unit.GetComponent<Unit>();
        unitScript.initStats(unitID, 1, 1, 1, 1, 1);
        AddUnit(unit);
        unitID++;

        unit = Instantiate(penguinViking, new Vector3(0, -3.5f, 0), transform.rotation);
        unitScript = unit.GetComponent<Unit>();
        unitScript.initStats(unitID, 1, 1, 2, 3, 1);
        AddUnit(unit);
        unitID++;
    }

    public void NextTurn()
    {
        // display action buttons

        Unit unitScript = units[turn].GetComponent<Unit>();
        unitScript.PrintID();

        // display unit actions

        // press end turn to move on to next unit

        // focus on and display movement range of unit
        unitScript.Move();

        startButton.SetActive(false);
        endButton.SetActive(true);
    }

    public void EndTurn()
    {
        mapManager.ClearHLTiles();

        // make sure control is lost of the last unit played
        Unit unitScript = units[turn].GetComponent<Unit>();
        unitScript.setMovable(false);

        if (turn == units.Count - 1) // reset turns
        {
            turn = 0;
        }
        else
        {
            turn++;
        }

        Debug.Log("Now Turn: " + turn);

        NextTurn();
    }

    public void AddUnit(GameObject unit)
    {
        units.Add(unit);
    }

    public void InitializeBattle()
    {
        SpawnSampleUnits();
        Debug.Log("Unit list count: " + units.Count);

        // order units based on initiative
        units = units.OrderByDescending(e => e.GetComponent<Unit>().GetInit()).ToList();

        turn = 0;

        Debug.Log(units.Count);

        Destroy(GameObject.Find("Button 1"));
        
        startButton.SetActive(true);
        // GameObject.Find("Button 3").SetActive(true);

    }

    //private int SortByInit(Unit unit1, Unit unit2)
    //{
    //    if(unit1.GetInit() >= unit2.GetInit())
    //    {
    //        return unit1;
    //    }
    //}

}
