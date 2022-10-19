using Assets.Scripts.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    


    private List<ActionButtonUI> actionButtonUiList;

    private void Awake()
    {
        actionButtonUiList = new List<ActionButtonUI>();
    }


    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        CreateUnitActionButtons();
        updateSelectedVisual();
    }

    private void CreateUnitActionButtons()
    {
        Unit selectedUnit = UnitActionSystem.Instance.getSelectedUnit();
        
        foreach (Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonUiList.Clear();

        foreach (BaseAction baseAction in selectedUnit.getBaseActionArray())
        {
            Debug.Log("UnitActionSYstem UI Base action list --> " + baseAction.GetActionName());
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.setBaseAction(baseAction);

            actionButtonUiList.Add(actionButtonUI);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
    }

    private void updateSelectedVisual()
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUiList)
        {
            actionButtonUI.updateSelectedVisual();
        }
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        updateSelectedVisual();
    }
}
