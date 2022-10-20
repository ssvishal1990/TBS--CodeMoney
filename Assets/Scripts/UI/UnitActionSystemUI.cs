using Assets.Scripts.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI actionPointstext;
    


    private List<ActionButtonUI> actionButtonUiList;

    private void Awake()
    {
        actionButtonUiList = new List<ActionButtonUI>();
    }


    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.onActionStarted += UnitActionSystem_OnActionStarted;
        CreateUnitActionButtons();
        updateSelectedVisual();
        UpadateActionPoints();
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
        UpadateActionPoints();
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
    
    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpadateActionPoints();
    }

    private void UpadateActionPoints()
    {
        Unit selectedUnit = UnitActionSystem.Instance.getSelectedUnit();
        actionPointstext.text = "Action points " + selectedUnit.getCurrentActionPoints().ToString();
    }
}
