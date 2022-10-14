using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HealthUpScript : MonoBehaviourPun
{
    HealthManager hm;
    public int healAmount = -1;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag != "Enemy")
        {
            var healthComponent = collision.GetComponent<HealthManager>();
            if (healthComponent != null)
            {
                hm = healthComponent;
                photonView.RPC("Heal", PhotonNetwork.LocalPlayer, healAmount);
                //healthComponent.TakeDamage(-1);
                //Destroy(gameObject);
            }
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    void Heal()
    {
        if (hm != null) hm.TakeDamage(healAmount);
    }
}
