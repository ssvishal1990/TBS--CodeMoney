using Assets.Scripts.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    private int maxShootDistance = 5;
    
    private float stateTimer;
    private bool canShootBullet;

    private Action spinCompletedDelegate;
    private Unit targetUnit;


    private enum State
    {
        Aiming,
        Shooting,
        CoolOff,
    }

    private State state;    

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.Aiming:
                AimAtTarget();
                break;
            case State.Shooting:
                InitiateShoot();
                break;
            case State.CoolOff:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void InitiateShoot()
    {
        if (canShootBullet)
        {
            Shoot();
            canShootBullet = false;
        }
    }

    private void AimAtTarget()
    {
        float characterRotateSpeed = 10f;
        Vector3 moveDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * characterRotateSpeed);
    }

    private void Shoot()
    {

        targetUnit.Damage();
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.CoolOff;
                float CoolOffStateTime = 0.1f;
                stateTimer = CoolOffStateTime;
                break;
            case State.CoolOff:
                ActionComplete();
                break;
        }
    }



    public override string GetActionName()
    {
        return "Shoot";
    }


    public override void TakeAction(GridPosition gridPosition, Action spinCompletedDelegate)
    {
        ActionStart(spinCompletedDelegate);


        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        float AimingStateTime = 0.1f;
        stateTimer = AimingStateTime;

        canShootBullet = true;

    }

    public override List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.isValidGridPosition(testGridPosition))
                {
                    // Shooting Range
                    continue;
                }
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxShootDistance)
                {
                    continue;
                }
                if (!LevelGrid.Instance.hasAnyUnitOnGridPosition(testGridPosition))
                {
                    //Grid Position is Empty || no unit
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    //Both Unit are on same team ?
                    continue;
                }

                validGridPositions.Add(testGridPosition);
                //Debug.Log(testGridPosition);
            }
        }
        return validGridPositions;
    }
}
