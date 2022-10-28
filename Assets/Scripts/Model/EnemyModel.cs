using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.AI;

public class EnemyModel : MonoBehaviourPun
{
    HealthManager healthMgr;

    public NavMeshAgent agent;
    public Transform targetTransform;
    public LayerMask whatIsGroundLayermask, whatIsPlayerLayermask;
    public int health;
    Animator anim;

    public CharacterModel[] characters;
    CharacterModel characterTarget;
    int targetIndex;
    bool isTargetIndexSet = false;


    public float sightRange, attackRange;

    //patrol
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public HealthManager HealthMgr { get => healthMgr; set => healthMgr = value; }

    // Start is called before the first frame update
    void Awake()
    {
        healthMgr = GetComponent<HealthManager>();
    }

    private void Start()
    {
        characters = FindObjectsOfType<CharacterModel>();
    }

    #region Enemy_Decision_Tree_Questions
    
    public bool IsPlayerInSightRange()
    {

        //player in sight/range
        bool playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayerLayermask);
        return playerInSightRange;

    }

    public bool IsPlayerInAttackRange()
    {

        bool playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayerLayermask);
        return playerInAttackRange;

    }

    #endregion
    #region Enemy_Decision_Tree_Actions
    public void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGroundLayermask))
            walkPointSet = true;
    }

    public void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    public void ChasePlayer()
    {

        if (photonView.IsMine && characters.Length >= 1)
        {
            agent.SetDestination(characters[targetIndex].transform.position);
        }

    }

    [PunRPC]
    void RequestTargetIndex(Player client)
    {

        /*int index = Random.Range(0, characters.Length-1);*/
        if (characters == null) Debug.Log("Characters es null");
        //characterTarget = characters[index];
        photonView.RPC("SetEnemyTargetIndex", client, UnityEngine.Random.Range(0, characters.Length));

    }

    void SetEnemyTargetIndex(int _index)
    {
        targetIndex = _index;
        //agent.SetDestination(_target.transform.position);
    }

    public void AttackPlayer()
    {
        if (characters.Length >= 1)
        {
            //enemy does not move
            agent.SetDestination(characters[targetIndex].transform.position);

            transform.LookAt(characters[targetIndex].transform);

            if (!alreadyAttacked)
            {
                anim.SetBool("Attack", true);
                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
    }
    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    #endregion


}
