using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpScript : MonoBehaviourPun
{
    HealthManager hm;
    public int healAmount = -1;

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.tag == TagManager.PLAYER_TAG)
            {
                var healthComponent = other.GetComponent<HealthManager>();
                if (healthComponent != null)
                {
                    hm = healthComponent;
                    PhotonView pv = other.gameObject.GetPhotonView();
                    if (pv != null)
                    {
                        pv.RPC("TakeDamage", pv.Owner, healAmount);
                    }
                }
                photonView.RPC("DestroyBottle", photonView.Owner);
            }
            else photonView.RPC("DestroyBottle", photonView.Owner);
        }
    }

    [PunRPC]
    void DestroyBottle()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
