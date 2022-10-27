using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Actions;
using System;

public class Unit : MonoBehaviour
{
    [SerializeField] private bool isEnemy;



    public static EventHandler OnAnyActionPointsChanged;

    private const int ACTION_POINTS_MAX = 2;
    private int actionPoints = ACTION_POINTS_MAX;

    

    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private HealthSystem healthSystem;
    private BaseAction[] baseActionArray;


    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
        healthSystem = GetComponent<HealthSystem>();
        //LevelGrid.Instance.SetUnitAtPosition(LevelGrid.Instance.getGridPosition(transform.position), this);
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.getGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtPosition(gridPosition, this);
        TurnSystem.Instance.onTurnChanged += TurnSystem_onTurnChanged;
        healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
    }

    private void Update()
    {
        UpdateCharacterPosition();
    }

    public void Damage()
    {
        Debug.Log(transform.name + "  Damaged");
    }

    internal Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    private void UpdateCharacterPosition()
    {
        GridPosition newgridPosition = LevelGrid.Instance.getGridPosition(transform.position);
        if (gridPosition != newgridPosition)
        {
            //Debug.Log("Change in grid Position detected");
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
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
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


    private void TurnSystem_onTurnChanged(object sender, EventArgs e)
    {
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = ACTION_POINTS_MAX;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy() => isEnemy;

    public void Damage(int damageAmount) => healthSystem.Damage(damageAmount);
}
