using Assets.Scripts.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;

    private void Start()
    {
        BaseAction.onAnyActionStarts += BaseAction_OnAnyActionStarts;
        BaseAction.onAnyActionCompletes += BaseAction_Completes;
        hideActionCamera();
    }

    private void BaseAction_Completes(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                hideActionCamera();
                break;
        }
    }

    private void BaseAction_OnAnyActionStarts(object sender, EventArgs e)
    {
        //Debug.Log("Inside Show Action Camera triggered");
        switch (sender)
        {
            case ShootAction shootAction:
                Unit ShooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.getTargetUnit();

                Vector3 cameraCharacterHeight = Vector3.up * 1.7f;
                Vector3 shootDir = (targetUnit.GetWorldPosition()
                        - ShooterUnit.GetWorldPosition()).normalized;

                float shoulderOffSetAmount = 0.5f;
                Vector3 shoulderOffSet = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffSetAmount;

                Vector3 actionCameraPosition = ShooterUnit.GetWorldPosition()
                    + cameraCharacterHeight
                    + shoulderOffSet
                    + (shootDir * -1);
                //Debug.Log("Show Action Camera triggered");
                actionCameraGameObject.transform.position = actionCameraPosition;
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);
                showActionCamera();
                break;
        }
    }

    private void showActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }

    private void hideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }
}
