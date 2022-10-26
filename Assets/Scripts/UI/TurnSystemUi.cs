using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private Button endTurnButton;
    [SerializeField] private GameObject enemyTurnSystemVisual;


    void Start()
    {
        UpdateTurnSystemUI();
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
        TurnSystem.Instance.onTurnChanged += TurnSystem_OnTurnChanged;
    }

    public void UpdateTurnSystemUI()
    {
        endTurnButton.onClick.AddListener(() =>
        {
            Debug.Log("Triggering next turn method");
            TurnSystem.Instance.NextTurn();
        });
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
        UpdateEndTurnButtonVisibility();
        UpdateEnemyTurnVisual();
    }

    private void UpdateTurnText()
    {
        turnNumberText.text = "TURN : " + TurnSystem.Instance.getCurrentTurnNumber().ToString();
    }

    private void UpdateEnemyTurnVisual()
    {
        enemyTurnSystemVisual.gameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateEndTurnButtonVisibility()
    {
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
    //public void 
}
