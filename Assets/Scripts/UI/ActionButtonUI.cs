using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Assets.Scripts.Actions;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedGameObject;

    private BaseAction baseAction;

    public void setBaseAction(BaseAction baseaction)
    {
        textMeshProUGUI.text = baseaction.GetActionName().ToUpper();
        this.baseAction = baseaction;

        button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.setSelectedAction(baseaction);
        });
    }


    public void updateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.getSelectedAction();
        selectedGameObject.SetActive(selectedBaseAction == this.baseAction);
    }

}
