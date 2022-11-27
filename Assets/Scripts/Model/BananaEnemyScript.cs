using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class BananaEnemyScript : Bullet
{

    HealthManager healthManager;
    private void Update()
    {
        if (photonView.IsMine) rotate();
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            var healthComponent = collision.collider.GetComponent<HealthManager>();
            if (healthComponent != null)
            {
                photonView.RPC("TakeDamage", PhotonNetwork.LocalPlayer, bulletDmg);
            }
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            PhotonNetwork.Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var healthComponent = other.GetComponent<HealthManager>();
            if (healthComponent != null)
            {
                healthManager = healthComponent;
                photonView.RPC("TakeDamage", PhotonNetwork.LocalPlayer, bulletDmg);
            }
            photonView.RPC("DestroyBullet", PhotonNetwork.LocalPlayer);
        }
        else
        {
            photonView.RPC("DestroyBullet", PhotonNetwork.LocalPlayer);
        }

    }

    [PunRPC]
    void DestroyBullet()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

    [PunRPC]
    void TakeDamage(int damage)
    {
        if (healthManager != null) healthManager.TakeDamage(damage);
    }

    void rotate()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }
}
