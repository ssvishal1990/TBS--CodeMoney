
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Actions
{
    public class SpinAction : BaseAction
    {


        private float totalSpinAmount;
        private Action spinCompletedDelegate;

        //private SpinCompletedDelegate spinCompletedDelegate;
        private void Update()
        {
            if (!isActive)
            {
                return;
            }
            Spin();
        }

        private void Spin()
        {
            float spinAddAmount = 360f * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
            totalSpinAmount += spinAddAmount;

            if (totalSpinAmount >= 360f)
            {
                ActionComplete();
            }
        }

        public override void TakeAction(GridPosition gridPosition, Action spinCompletedDelegate)
        {
            totalSpinAmount = 0f;
            ActionStart(spinCompletedDelegate);
        }

        public override string GetActionName()
        {
            return "Spin";
        }

        public override List<GridPosition> GetValidActionGridPosition()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();
            GridPosition unitGridPosition = unit.GetGridPosition();

            return new List<GridPosition>()
            {
                unitGridPosition
            };
        }

        public override int getActionPointsCost()
        {
            return 2;
        }


    }
}

