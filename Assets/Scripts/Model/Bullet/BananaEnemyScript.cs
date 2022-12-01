using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaEnemyScript : Bullet // TODO # Note: Este script quedará deprecado al migrar lógica de los nuevos enemies. Se utilizará el script Bullet que utiliza el Player, script el cual se encuentra correctamente sincronizado.
{

    HealthManager healthManager;
    private void Update()
    {
        if (photonView.IsMine) rotate();
    }

    //No deberìa tener los dos tipos de colisiones. Dejar solamente el trigger, y pasar por string el tag del target. Para no tener que repetir el còdigo
    private void OnCollisionEnter(Collision collision)
    {
        //if(photonView.Ismine)
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
        //
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
    void TakeDamage(int damage) //Usar el mètodo del healthcontroller. De ser necesario acceder al photonView del healthmanager (healthManager.photonView.RPC("",,);
    {
        if (healthManager != null) healthManager.TakeDamage(damage);
    }

    void rotate()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }
}
