using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviourPun // TODO # Note: Se modificará la lógica en este script con la nueva lógica de los enemies.
{

    EnemyModel enemyModel;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            enemyModel = GetComponent<EnemyModel>();
        }
    }

    private void Update()
    {
        if (photonView.IsMine && enemyModel.Target != null)
        {
            if (Vector3.Distance(transform.position, enemyModel.Target.transform.position) > enemyModel.AttackRange)
                enemyModel.SeekCharacter();
            else enemyModel.AttackCharacter();
        }
    }

}
