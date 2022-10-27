using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDeath;
    [SerializeField] public int health = 100;
    [SerializeField] private int healthMax = 100;

    public event EventHandler OnDamage;
    public void Damage(int damageAmount)
    {
        Debug.Log("Unit Damaged with damage  :  " + damageAmount);
        health -= damageAmount;
        if (health < 0)
        {
            health = 0;
        }

        if (health == 0)
        {
            Die();
        }
        OnDamage?.Invoke(this, EventArgs.Empty);
    }

    private void Die()
    {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

    public float getHealthNormalized()
    {
        return (float)health / healthMax;
    }
}
