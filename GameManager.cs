using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject penguinViking;

    public int turn;
    [SerializeField] public List<GameObject> units;
    private int unitID = 0;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SpawnSampleUnits()
    {
        Unit unitScript;
        unitScript = Instantiate(penguinViking, new Vector3(0, 0, 0), transform.rotation).GetComponent<Unit>();
        unitScript.initStats(unitID);
        unitID++;
        unitScript = Instantiate(penguinViking, new Vector3(-0.5f, 0.75f, 0), transform.rotation).GetComponent<Unit>();
        unitScript.initStats(unitID);
        unitID++;
        unitScript = Instantiate(penguinViking, new Vector3(0, -1.5f, 0), transform.rotation).GetComponent<Unit>();
        unitScript.initStats(unitID);
        unitID++;
    }

    public void Turn()
    {
        Unit unitScript = units[turn].GetComponent<Unit>();
        unitScript.PrintID();

        // display movement range of unit

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
        turn = 0;
    }
}
