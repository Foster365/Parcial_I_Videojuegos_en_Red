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
        if (photonView.IsMine)
        {
            SetStartingHealth();
        }
        else
        {
            photonView.RPC("RequestCurrentHealth", photonView.Owner, PhotonNetwork.LocalPlayer);
        }
    }

    [PunRPC]
    public void RequestCurrentHealth(Player client)
    {
        photonView.RPC("UpdateHealth", client, currentHealth);
    }

    [PunRPC]
    public void UpdateHealth(int life)
    {
        currentHealth = life;
        float currentHealthPct = (float)currentHealth / (float)maxHealth;
        OnHealthPctChanged(currentHealthPct);
    }

    void SetStartingHealth()
    {
        currentHealth = maxHealth;
    }

    [PunRPC]
    public void TakeDamage(int amount)
    {
        if (photonView.IsMine)
        {
            currentHealth -= amount;
            Debug.Log(gameObject.name + "Health: " + currentHealth);
            if (currentHealth <= 0)
            {
                //death
                isDead = true;
                Kill();
            }
            photonView.RPC("UpdateHealth", RpcTarget.All, currentHealth);
            //float currentHealthPct = (float)currentHealth / (float)maxHealth;
            //OnHealthPctChanged(currentHealthPct);
        }
        else
        {
            photonView.RPC("TakeDamage", photonView.Owner, amount);
        }
    }

    void Kill()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }
}
