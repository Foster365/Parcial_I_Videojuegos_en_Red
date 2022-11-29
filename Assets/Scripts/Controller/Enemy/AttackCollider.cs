using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviourPun
{
    public int attackColliderDamage;
    HealthManager healthManager;
    [SerializeField] string targetTag;


    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {

            if (other.tag == targetTag)
            {
                var healthComponent = other.GetComponent<HealthManager>();
                if (healthComponent != null)
                {
                    healthManager = healthComponent;
                    //healthManager.TakeDamage(bulletDmg);
                    PhotonView pv = other.gameObject.GetPhotonView();
                    Debug.Log("PV: " + pv);
                    if (pv != null)
                    {
                        pv.RPC("TakeDamage", pv.Owner, attackColliderDamage);
                        Debug.Log("Target health: " + healthComponent.CurrentHealth);
                    }
                }
            }

        }
    }
}
