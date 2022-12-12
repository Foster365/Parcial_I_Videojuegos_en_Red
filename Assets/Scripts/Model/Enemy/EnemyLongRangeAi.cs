using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLongRangeAi : MonoBehaviourPun // TODO # Note: Se eliminará este script cuando se migre la nueva lógica de los enemies.
{
    public NavMeshAgent agent;
    public LayerMask whatIsGround, whatIsPlayer;
    public int health;
    Animator anim;
    public Transform firePoint;
    public int bulletForce;

    CharacterModel[] characters;
    public GameObject player;
    CharacterModel characterTarget;
    int targetIndex;
    bool isTargetIndexSet = false;

    //patrol
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //attacking
    public float timeBetweenAttacks;

    //states
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        //if (photonView.IsMine)
        //{
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            
        //}
    }

    private void Start()
    {
        characters = FindObjectsOfType<CharacterModel>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<GameObject>();

    }

    private void Update()
    {
        Debug.Log("player name: " + player);
        if (characters != null)
        {
            Debug.Log("CHARACTERS COUNT: " + characters.Length);
            //player in sight/range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            //if (!playerInSightRange && !playerInAttackRange) Patrolling();
            //if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange)
            {
                AttackPlayer();
            } 


        }
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {

        if (photonView.IsMine && characters.Length >= 1)
        {
            agent.SetDestination(characters[targetIndex].transform.position);
        }
    }

    [PunRPC]
    void RequestTargetIndex(Player client)
    {
        if (characters == null) Debug.Log("Characters es null");
        photonView.RPC("SetEnemyTargetIndex", client, UnityEngine.Random.Range(0, characters.Length));
    }

    void SetEnemyTargetIndex(int _index)
    {
        targetIndex = _index;
    }

    private void AttackPlayer()
    {
        //if (characters.Length >= 1)
        //{
            //enemy does not move
            agent.SetDestination(player.transform.position);

            transform.LookAt(characters[targetIndex].transform);
            Debug.Log("attempted to attack");
            Shoot();
        //}
    }
    void Shoot()
    {
        Debug.Log("shoot");
        GameObject bullet = PhotonNetwork.Instantiate("BananaBulletEnemy", firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
