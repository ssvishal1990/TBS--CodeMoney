using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;

    private void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnDamage += healthSystem_OnDamage;
        UpdateActionPointsText();
        updateHealthBar();
    }

    private void healthSystem_OnDamage(object sender, EventArgs e)
    {
        updateHealthBar();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void UpdateActionPointsText()
    {
        actionPointsText.text = unit.getCurrentActionPoints().ToString();
    }

    private void updateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.getHealthNormalized();
    }
}
