using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Actions
{
    public abstract  class BaseAction : MonoBehaviour
    {
        protected Unit unit;
        protected bool isActive;
        private Action onActionComplete;

        protected virtual void Awake()
        {
            unit = GetComponent<Unit>();
        }

        public abstract string GetActionName();

        public abstract void TakeAction(GridPosition gridPosition, Action onActionCompleted);

        public bool isValidActionGridPosition(GridPosition gridPosition)
        {
            return GetValidActionGridPosition().Contains(gridPosition);
        }

        public abstract List<GridPosition> GetValidActionGridPosition();

        public virtual int getActionPointsCost()
        {
            return 1;
        }

        protected void ActionStart(Action onActionComplete)
        {
            isActive = true;
            this.onActionComplete = onActionComplete;
        }

        protected void ActionComplete()
        {
            isActive = false;
            onActionComplete();
        }
    }

   
}