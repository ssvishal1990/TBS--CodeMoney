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
        [SerializeField] private Animator unitAnimator;
        [SerializeField] private int maxMoveDistance = 4;

        private Vector3 targetPosition;
        private Action onActionComplete;

        protected override void Awake()
        {
            base.Awake();
            targetPosition = transform.position;
        }

        void Start()
        {

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
            unitAnimator.SetBool("isWalking", true);
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            if (!checkIfReached())
            {
                transform.position += moveDirection * Time.deltaTime * moveSpeed;
                //transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * characterRotateSpeed);
                GetValidActionGridPosition();
                unitAnimator.SetBool("isWalking", true);
            }
            else
            {
                unitAnimator.SetBool("isWalking", false);
                isActive = false;
                onActionComplete();
            }
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * characterRotateSpeed);
        }

        public void Move(Vector3 targetPosition)
        {
            this.targetPosition = targetPosition;
        }

        public void Move(GridPosition gridPosition, Action onActionComplete)
        {
            this.onActionComplete = onActionComplete;
            this.targetPosition = LevelGrid.Instance.getWorldPosition(gridPosition);
            isActive = true;
            //this.targetPosition = targetPosition;
        }



        private bool checkIfReached()
        {
            int distance = (int)Mathf.Round(Vector3.Distance(transform.position, targetPosition));
            if (distance == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<GridPosition> GetValidActionGridPosition()
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

        public bool isValidActionGridPosition(GridPosition gridPosition)
        {
            return GetValidActionGridPosition().Contains(gridPosition);
        }

        public override string GetActionName()
        {
            return "Move";
        }

       
    }
}

