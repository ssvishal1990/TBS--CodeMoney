using Assets.Scripts.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    private enum State
    {
        WaitingForTurn,
        TakingTurn,
        Busy,
    }

    private State state;

    private float timer;

    private void Awake()
    {

        state = State.WaitingForTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.onTurnChanged += TurnSystem_onTurnChanged;
    }

    private void TurnSystem_onTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2f;
        }
        
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch (state) 
        {
            case State.WaitingForTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    if (TakeEnemyAIAction(setStateTakingTurn))
                    {
                        state = State.Busy;
                    }else
                    {
                        // No more enemies has action that they can take
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }
    }

    private void setStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }
    private bool TakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        Debug.Log("Take enemy Ai Action");
        foreach (Unit enemyUnit in UnitManager.Instance.getEnemyUnitsList())
        {
            if (TakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {
                return true;
            }
        }
        return false;
    }

    private bool TakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        SpinAction spinAction = enemyUnit.getSpinAction();
        GridPosition actionGridPosition = enemyUnit.GetGridPosition();
        if (!spinAction.isValidActionGridPosition(actionGridPosition))
        {
            return false;
        }
        if (!enemyUnit.TrySpendingActionPointToPerformAction(spinAction))
        {
            return false;
        }
        Debug.Log("Spin ACtion");
        spinAction.TakeAction(actionGridPosition, onEnemyAIActionComplete);
        return true;
    }
}
