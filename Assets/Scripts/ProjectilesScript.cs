using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class ProjectilesScript : MonoBehaviourPun
{
    public Transform firePoint;
    public Animator anim;

    public float bulletForce = 20f;

    private void Start()
    {
        if (!photonView.IsMine) Destroy(this);
        anim.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
        if (Input.GetButtonDown("Fire1"))
        {
            //Shoot();
            photonView.RPC("Shoot", PhotonNetwork.LocalPlayer);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            anim.SetBool("Punch", true);
        }

        }
    }
    [PunRPC]
    void Shoot()
    {
        GameObject bullet = PhotonNetwork.Instantiate("BananaBullet", firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
    }
}
