using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviourPun
{
    public int maxHealth = 3;
    int currentHealth;
    CharacterView charView;

    bool isDead = false;

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public bool IsDead { get => isDead; set => isDead = value; }

    public event Action<float> OnHealthPercentHasChanged = delegate { };

    private void Awake()
    {
        charView = GetComponent<CharacterView>();
    }

    void Start()
    {
        if (photonView.IsMine) SetStartingHealth();
        else photonView.RPC("RequestCurrentHealth", photonView.Owner, PhotonNetwork.LocalPlayer);
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
        OnHealthPercentHasChanged(currentHealthPct);
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
            charView.HandleHitAnim(true);
            if (currentHealth <= 0)
            {
                //death
                isDead = true;
                photonView.RPC("Die", photonView.Owner);
            }
            photonView.RPC("UpdateHealth", photonView.Owner, currentHealth);
            StartCoroutine(WaitUntiDeactivateAnim());
            charView.HandleHitAnim(false);
            //float currentHealthPct = (float)currentHealth / (float)maxHealth;
            //OnHealthPctChanged(currentHealthPct);
        }
        //else
        //{
        //    photonView.RPC("TakeDamage", photonView.Owner, amount);
        //}
    }

    [PunRPC]
    void Die()
    {
        charView.HandleDeathAnim(true);
        StartCoroutine(WaitUntiDeactivateAnim());
        PhotonNetwork.Destroy(gameObject);
    }

    IEnumerator WaitUntiDeactivateAnim()
    {
        yield return new WaitForSeconds(1f);
    }
}
