using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class HealthManager : MonoBehaviourPun
{
    public int maxHealth = 3;
    int currentHealth;

    bool isDead = false;

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public bool IsDead { get => isDead; set => isDead = value; }

    public event Action<float> OnHealthPctChanged = delegate { };

    void Start()
    {
        if (!photonView.IsMine) Destroy(this);
        photonView.RPC("SetStartingHealth", RpcTarget.All);
    }

    [PunRPC]
    void SetStartingHealth() 
    {
        currentHealth = maxHealth;
    }

    [PunRPC]
    public void TakeDamage(int amount)
    {
        if(photonView.IsMine)
        {
            currentHealth -= amount;
            Debug.Log(gameObject.name + "Health: " + currentHealth);
            if (currentHealth <= 0)
            {
                //death
                isDead = true;
                photonView.RPC("Kill", RpcTarget.All);
            }

            float currentHealthPct = (float)currentHealth / (float)maxHealth;
            OnHealthPctChanged(currentHealthPct);
        }
    }

    [PunRPC]
    void Kill()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }
}
