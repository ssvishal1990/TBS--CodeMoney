using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Actions;

public class Unit : MonoBehaviour
{
    private int actionPoints = 2;

    

    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;


    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
        //LevelGrid.Instance.SetUnitAtPosition(LevelGrid.Instance.getGridPosition(transform.position), this);
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.getGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtPosition(gridPosition, this);
    }
    private void Update()
    {
        UpdateCharacterPosition();
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Move(MouseWorld.GetPosition());
        //}
    }

    private void UpdateCharacterPosition()
    {
        GridPosition newgridPosition = LevelGrid.Instance.getGridPosition(transform.position);
        if (gridPosition != newgridPosition)
        {
            Debug.Log("Change in grid Position detected");
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newgridPosition);
            gridPosition = newgridPosition;
        }
    }

    

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public MoveAction getMoveAction()
    {
        return moveAction;
    }
    public SpinAction getSpinAction()
    {
        return spinAction;
    }

    public BaseAction[] getBaseActionArray()
    {
        return baseActionArray;
    }

    public bool CanSpendActionPointsToTakeActions(BaseAction baseAction)
    {
        if (actionPoints >= baseAction.getActionPointsCost())
        {
            return true;
        }else
        {
            return false;
        }
        
    }

    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;
    }

    public bool TrySpendingActionPointToPerformAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeActions(baseAction))
        {
            SpendActionPoints(baseAction.getActionPointsCost());
            return true;
        }else
        {
            return false;
        }

    }

    public int getCurrentActionPoints()
    {
        return actionPoints;
    }
}
