using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Photon.Pun;
using Photon.Realtime;

public class EnemyController : MonoBehaviourPun
{

    EnemyModel enemyModel;
    
    //states
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        if (!photonView.IsMine) Destroy(this);
        enemyModel = GetComponent<EnemyModel>();
    }

    private void Update()
    {
        if(enemyModel.characters != null)
        {

            if (!enemyModel.IsPlayerInSightRange() && !enemyModel.IsPlayerInAttackRange()) enemyModel.Patrolling();
            if (enemyModel.IsPlayerInSightRange() && !enemyModel.IsPlayerInAttackRange()) enemyModel.ChasePlayer();
            if (enemyModel.IsPlayerInAttackRange() && enemyModel.IsPlayerInSightRange()) enemyModel.AttackPlayer();

        }
    }

    void OnDrawGizmosSelected()
    {
        if(photonView.IsMine)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyModel.attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, enemyModel.sightRange);
        }
    }
}
