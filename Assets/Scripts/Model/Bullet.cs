using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class Bullet : MonoBehaviourPun
{
    public float rotateSpeed;
    public int bulletDmg;
    HealthManager healthManager;

    private void OnCollisionEnter(Collision collision)
    {
        //if(photonView.IsMine)
        //{
        if (collision.collider.tag == "Enemy")
        {
            var healthComponent = collision.collider.GetComponent<HealthManager>();
            if (healthComponent != null)
            {
                photonView.RPC("TakeDamage", RpcTarget.All, bulletDmg);
            }
            DestroyBullet();
        }
        else
        {
            DestroyBullet();
        }

        //}
    }

    [PunRPC]
    void DestroyEnemy()
    {

        PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {

            if (other.tag == "Enemy")
            {
                var healthComponent = other.GetComponent<HealthManager>();
                if (healthComponent != null)
                {
                    healthManager = healthComponent;
                    healthManager.TakeDamage(bulletDmg);
                    // photonView.RPC("TakeDamage", PhotonNetwork.LocalPlayer, bulletDmg);
                }
                DestroyBullet();
            }
            else DestroyBullet();

        }
    }

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
