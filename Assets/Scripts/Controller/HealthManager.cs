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

    public bool isDead = false;

    public event Action<float> OnHealthPctChanged = delegate { };

    void Start()
    {
        if (!photonView.IsMine)
        {
            Destroy(this);
        }
        photonView.RPC("SetStartingHealth", PhotonNetwork.LocalPlayer);
    }

    [PunRPC]
    void SetStartingHealth() 
    {
        currentHealth = maxHealth;
    }

    [PunRPC]
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log(gameObject.name + "Health: " + currentHealth);
        if (currentHealth <= 0)
        {
            //death
            isDead = true;
            photonView.RPC("Kill", PhotonNetwork.LocalPlayer);
        }

        float currentHealthPct = (float)currentHealth / (float)maxHealth;
        OnHealthPctChanged(currentHealthPct);
    }

    [PunRPC]
    void Kill()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }
}
