// Unit Initiative that determines turn order, status effects, hp (still alive), exp, and Tracking Component (GameManager tracks unit existence and status)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitGeneral : MonoBehaviour
{
    protected int id;
    protected int status; // friend or enemy
    protected int init;
    protected int hp;
    protected int actions;

    // initialize general stats all units should have
    public void initGenStats(int id_val, int status_val = 0, int init_val = 0, int hp_val = 0, int act_val = 2)
    {
        id = id_val;
        status = status_val; // 0 for player, 1 for enemy
        init = init_val;
        hp = hp_val;
        actions = act_val;
    }

    // focus camera on this unit
    public void Focus()
    {
        Camera.main.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Camera.main.transform.position.z);
    }

    // adjust hp based on dmg received, kill unit if hp reaches 0
    public void TakeDmg(int dmg) // NOT DONE
    {
        hp -= dmg;

        if(hp <= 0)
        {
            Debug.Log("Dead");
            Death();
        }
    }

    // the unit is dead
    public void Death()
    {
        // set its occupancy to false
        UnitMovement movementUnit = gameObject.GetComponent<UnitMovement>();
        movementUnit.SetOcc(false);

        // remove unit from lists
        Destroy(gameObject);
    }

    public int GetInit()
    {
        return init;
    }

    public int GetHP()
    {
        return hp;
    }

    public int GetID()
    {
        return id;
    }

    public int GetStatus()
    {
        return status;
    }

    public int GetActions()
    {
        return actions;
    }

    public void SetActions(int act_val)
    {
        actions = act_val;
    }
}
