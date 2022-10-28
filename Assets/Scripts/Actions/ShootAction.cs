using Assets.Scripts.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    [SerializeField] private int damageAmount;
    [SerializeField] private float AimingStateTime = 0.1f;
    [SerializeField] private float shootingStateTime = 0.2f;
    [SerializeField] private float CoolOffStateTime = 0.1f;
    [SerializeField] private float stateTimer;
    [SerializeField] private int maxShootDistance = 5;
    
    
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

    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit ShootingUnit;
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        PerformStateActions();
    }

    private void PerformStateActions()
    {
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
        OnShoot?.Invoke(this, new OnShootEventArgs()
        {
            targetUnit = targetUnit,
            ShootingUnit = unit
        });
        targetUnit.Damage(damageAmount);
        targetUnit.Damage();
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.CoolOff;
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
        


        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        
        stateTimer = AimingStateTime;

        canShootBullet = true;
        ActionStart(spinCompletedDelegate);

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

    public Unit getTargetUnit()
    {
        return targetUnit;
    }

    public int getMaxShootDistance()
    {
        return maxShootDistance;
    }
}
