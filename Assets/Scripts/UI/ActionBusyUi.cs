using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUi : MonoBehaviour
{
    [SerializeField] private GameObject onBusyGameObject;
    void Start()
    {
        UnitActionSystem.Instance.onBusyStatusChange += UnitActionSystem_onBusyStateChange;
        Hide();
    }


    private void UnitActionSystem_onBusyStateChange(object sender, bool isBusy)
    {
        //Debug.Log("Change in busy state detected --> " + UnitActionSystem.Instance.getBusyState());
        if (isBusy)
        {
            Show();
        }else
        {
            Hide();
        }
    }


    private void Show()
    {
        onBusyGameObject.SetActive(true);
    }

    private void Hide()
    {
        onBusyGameObject.SetActive(false);
    }
}
