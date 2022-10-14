using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class HealthManager : MonoBehaviourPun
{
    public int maxHealth = 3;
    public int currentHealth;

    public event Action<float> OnHealthPctChanged = delegate { };

    void Start()
    {
        //rpc init health
        currentHealth = maxHealth;
    }

    //rpctake damage
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Health: " + currentHealth);
        if (currentHealth <= 0)
        {
            //death
            Destroy(gameObject);
        }

        float currentHealthPct = (float)currentHealth / (float)maxHealth;
        OnHealthPctChanged(currentHealthPct);
    }
}
