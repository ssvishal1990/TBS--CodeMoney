using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Actions
{
    public class MoveAction : BaseAction
{
        [SerializeField] float moveSpeed = 4f;
        [SerializeField] private float characterRotateSpeed = 4f;
        [SerializeField] private int maxMoveDistance = 4;

        private Vector3 targetPosition;
        private Action onActionComplete;

        public event EventHandler OnStartMoving;
        public event EventHandler OnStopMoving;

        protected override void Awake()
        {
            base.Awake();
            targetPosition = transform.position;
        }

        void Update()
        {
            UpdateCharacterPosition();
        }


        private void UpdateCharacterPosition()
        {
            if (!isActive)
            {
                return;
            }
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            if (!checkIfReached())
            {
                transform.position += moveDirection * Time.deltaTime * moveSpeed;
                GetValidActionGridPosition();
            }
            else
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * characterRotateSpeed);
        }

        public void Move(Vector3 targetPosition)
        {
            this.targetPosition = targetPosition;
        }

        override public void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            OnStartMoving?.Invoke(this, EventArgs.Empty);
            this.targetPosition = LevelGrid.Instance.getWorldPosition(gridPosition);
            ActionStart(onActionComplete);
        }



        private bool checkIfReached()
        {
            float stoppingDistance = .1f;
            float  distance = Vector3.Distance(transform.position, targetPosition);
            if (distance >  stoppingDistance)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override List<GridPosition> GetValidActionGridPosition()
        {
            List<GridPosition> validGridPositions = new List<GridPosition>();

            GridPosition unitGridPosition = unit.GetGridPosition();
            for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
            {
                for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.Instance.isValidGridPosition(testGridPosition))
                    {
                        // If GridLocation is within Bounds
                        continue;
                    }

                    if (unitGridPosition == testGridPosition)
                    {
                        // Same position is where unit already is
                        continue;
                    }

                    if (LevelGrid.Instance.hasAnyUnitOnGridPosition(testGridPosition))
                    {
                        //Grid Position already occupied
                        continue;
                    }
                    validGridPositions.Add(testGridPosition);
                    //Debug.Log(testGridPosition);
                }
            }
            return validGridPositions;
        }


        public override string GetActionName()
        {
            return "Move";
        }

       
    }
}

