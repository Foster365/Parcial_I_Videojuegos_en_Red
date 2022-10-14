using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float rotateSpeed;
    public int bulletDmg;
    private void Update()
    {
        rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            var healthComponent = collision.collider.GetComponent<HealthManager>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(bulletDmg);
            }
            Destroy(gameObject);
        }
        else
        {
          Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            var healthComponent = other.GetComponent<HealthManager>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(bulletDmg);
            }
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void rotate()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }
}
