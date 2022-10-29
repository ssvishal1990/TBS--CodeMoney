using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }
    private List<Unit> unitList;
    private List<Unit> friendlyUnitList;
    private List<Unit> enemyUnitList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than 1 Unit manager " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
        
    }
    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDeath += Unit_OnAnyUnitDeath;
    }

    private void Unit_OnAnyUnitDeath(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Remove(unit);
        if (unit.IsEnemy())
        {
            enemyUnitList.Remove(unit);
        }
        else
        {
            friendlyUnitList.Remove(unit);
        }
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        
        Unit unit = sender as Unit;

        unitList.Add(unit);
        if (unit.IsEnemy())
        {
            //Debug.Log("Unit Enemy spawned");
            enemyUnitList.Add(unit);
        }else
        {
            //Debug.Log("Unit friendly  spawned");
            friendlyUnitList.Add(unit);
        }
    }

    public List<Unit> getUnitList()
    {
        return unitList;
    }
    public List<Unit> getFriendlyUnitsList()
    {
        return friendlyUnitList;
    }
    public List<Unit> getEnemyUnitsList()
    {
        return enemyUnitList;
    }
}
