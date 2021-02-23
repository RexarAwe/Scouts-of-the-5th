// Unit Initiative that determines turn order, status effects and hp, and Tracking Component (GameManager tracks unit existence and status)

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

    // initialize general stats all units should have
    public void initStats(int id_val, int init_val = 0, int hp_val = 0)
    {
        id = id_val;
        init = init_val;
        hp = hp_val;
    }

    // focus camera on this unit
    public void Focus()
    {
        Camera.main.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Camera.main.transform.position.z);
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

}
