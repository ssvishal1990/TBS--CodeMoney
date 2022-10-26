using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    private GridSystem gridSystem;

    [SerializeField] private Transform gridDebugObjectPrefab;

    public static LevelGrid Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than 1 unit action system  " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        gridSystem = new GridSystem(10, 10, 2f);
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
        //gridSystem = new GridSystem(10, 10, 2f);
        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    // had to move this to start due to the conflic
    private void Start()
    {

    }

    public void AddUnitAtPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        //Debug.Log(" Checking Unit at level Grid  --> " + unit);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        //Debug.Log("Unit moved Grid Position method , From => " + fromGridPosition + " to -> " + toGridPosition);
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtPosition(toGridPosition, unit);
    }

    public GridPosition getGridPosition(Vector3 Pos) => gridSystem.GetGridPosition(Pos);

    public Vector3 getWorldPosition(GridPosition Pos) => gridSystem.GetWorldPosition(Pos);

    public bool isValidGridPosition(GridPosition gridPosition) => gridSystem.isValidGridPosition(gridPosition);

    public int getHeight() => gridSystem.getHeight();

    public int getWidth() => gridSystem.getWidhth();
    
    public bool hasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }



}
