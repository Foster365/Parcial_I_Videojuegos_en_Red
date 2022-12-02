using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShooting : MonoBehaviour
{
    public GameObject _player;
    float dist;
    public float howClose;
    public Transform head, barrel;
    public float fireRate, nextFire;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        Debug.Log("player name is:" + _player.name);
        dist = Vector3.Distance(_player.gameObject.transform.position, transform.position);
        if (dist <= howClose)
        {
            head.LookAt(_player.gameObject.transform);
            if(Time.time >= nextFire)
            {
                nextFire = Time.time + 1f / fireRate;
                Shoot();

            }
        }
    }

    void Shoot()
    {
        GameObject bullet = PhotonNetwork.Instantiate("BananaBulletEnemy", barrel.position, head.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(head.forward * 1500);
        Destroy(bullet, 10);
    }
}
