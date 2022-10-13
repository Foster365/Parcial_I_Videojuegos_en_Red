using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesScript : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Animator anim;

    public float bulletForce = 20f;

    private void Start()
    {
        anim.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            anim.SetBool("Punch", true);
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
    }
}
