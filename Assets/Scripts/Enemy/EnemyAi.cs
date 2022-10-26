using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    private float timer;


    private void Start()
    {
        TurnSystem.Instance.onTurnChanged += TurnSystem_onTurnChanged;
    }

    private void TurnSystem_onTurnChanged(object sender, EventArgs e)
    {
        timer = 2f;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            TurnSystem.Instance.NextTurn();
        }
    }
}
