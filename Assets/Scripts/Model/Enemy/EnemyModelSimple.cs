using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModelSimple : MonoBehaviour
{
    Transform target;

    private void Start()
    {
        target = GameObject.FindObjectOfType<CharacterModel>().transform;
    }

    public void Move()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir * Time.deltaTime);
    }
}
