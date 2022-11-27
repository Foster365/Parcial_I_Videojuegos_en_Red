using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviourPun
{
    public float rotateSpeed;
    public int bulletDmg;
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
                        pv.RPC("TakeDamage", pv.Owner, bulletDmg);
                        Debug.Log("Target health: " + healthComponent.CurrentHealth);
                    }
                }
                photonView.RPC("DestroyBullet", photonView.Owner);
            }
            else photonView.RPC("DestroyBullet", photonView.Owner);

        }
    }

    [PunRPC]
    void DestroyEnemy()
    {

        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    void DestroyBullet()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

    void Rotate()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }
}
