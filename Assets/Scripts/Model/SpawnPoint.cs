using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPoint : MonoBehaviourPun
{
    bool isAvaiable = true;
    [SerializeField] float gizmosRadius;

    public bool IsAvaiable { get => isAvaiable; set => isAvaiable = value; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, gizmosRadius);
    }
}
