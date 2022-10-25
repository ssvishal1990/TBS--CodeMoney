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


    void Start()
    {
        UpdateTurnSystemUI();
        UpdateTurnText();
        TurnSystem.Instance.onTurnChanged += TurnSystem_OnTurnChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTurnSystemUI()
    {
        endTurnButton.onClick.AddListener(() =>
        {
            Debug.Log("Triggering next turn method");
            TurnSystem.Instance.NextTurn();
        });
        //endTurnButton.onClick.AddListener(() =>
        //{

        //});

    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
    }

    private void UpdateTurnText()
    {
        turnNumberText.text = "TURN : " + TurnSystem.Instance.getCurrentTurnNumber().ToString();
    }

    //public void 
}
