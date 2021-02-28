// handles combat including atk, rng, dmg and def, also displays after checking if any enemies are in range to show indicators

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitAttack : MonoBehaviour
{
    private int atk;
    private int rng;
    private int def;

    private bool atkAble = false;

    private GameManager gameManager;
    private MapManager mapManager;
    private UnitMovement movementUnit;

    Tilemap tilemap;

    void Start()
    {
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        mapManager = GameObject.Find("Map Manager").GetComponent<MapManager>();
        movementUnit = gameObject.GetComponent<UnitMovement>();
    }

    protected void Update()
    {
        // onclick, if tile is movable, moves unit there and updates location
        if (Input.GetMouseButtonDown(0))
        {
            if (atkAble)
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int tileCoordinate = tilemap.WorldToCell(mouseWorldPos);

                if (mapManager.GetAtkableStatus(tileCoordinate))
                {
                    // perform attack and defend rolls
                    if(gameManager.GetUnit(tileCoordinate))
                    {
                        Attack(gameManager.GetUnit(tileCoordinate));
                        atkAble = false;
                    }
                }
            }
        }
    }

    public void setAtkStats(int atk_val, int rng_val, int def_val)
    {
        atk = atk_val;
        rng = rng_val;
        def = def_val;
    }

    // Given a location, indicate all enemies on tiles that can be attacked
    public void ShowAttacks()
    {
        mapManager.CheckAttack(movementUnit.getLoc(), rng);
        atkAble = true;
    }

    // roll hit and dmg, then adjust hp accordingly
    private void Attack(GameObject target)
    {
        //  is a hit and crit, nat 0 and 1 is an automatic miss

        //UnitGeneral generalUnit = target.GetComponent<UnitGeneral>();
        //Debug.Log(generalUnit.GetID());

        UnitAttack attackUnit = target.GetComponent<UnitAttack>();
        Debug.Log("Target ATK: " + attackUnit.getAtk());
        
        int hitPct = 50 + atk - attackUnit.getDef();

        int roll = Random.Range(0, 100);

        Debug.Log("Roll: " + roll);

        if (roll >= 98) // nat 98 and 99 is automatic miss
        {
            // do nothing since miss
        }
        else if (roll <= 1) // automatic hit and crit (deal double dmg)
        {
            InflictDmg(target);
        }
        else if (roll <= hitPct) // roll dmg and adjust target hp
        {
            InflictDmg(target);
        }
    }

    // roll dmg e.g. 1d6+2, 1 is num, dice is 6, mod is 2
    private int RollDmg(int num, int dice, int mod)
    {
        return 5;
    }

    private void InflictDmg(GameObject target)
    {

    }

    public int getAtk()
    {
        return atk;
    }

    public int getRng()
    {
        return rng;
    }

    public int getDef()
    {
        return def;
    }
}
