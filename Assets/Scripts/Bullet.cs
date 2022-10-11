using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float rotateSpeed;
    private void Update()
    {
        rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    void rotate()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }
}
