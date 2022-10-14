using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Photon.Pun;
using Photon.Realtime;

public class EnemyLongRangeAi : MonoBehaviourPun
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public int health;
    Animator anim;
    public Transform firePoint;
    public int bulletForce;

    CharacterModel[] characters;
    CharacterModel characterTarget;
    int targetIndex;
    bool isTargetIndexSet = false;

    //patrol
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //states
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        characters = FindObjectsOfType<CharacterModel>();
    }

    private void Update()
    {
        if (characters != null)
        {
            Debug.Log("CHARACTERS COUNT: " + characters.Length);
            //player in sight/range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange) Patrolling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) AttackPlayer();

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

        //var listCharacters = new List<CharacterModel>();

        //for (int i = 0; i < characters.Length; i++)
        //{
        //    var curr = characters[i];
        //    if (curr == characterTarget) continue;
        //    listCharacters.Add(curr);
        //}
        //if (listCharacters.Count > 1) //Pruebo con 1, sino bajo el range del random.range
        //{
        //    SetTarget(listCharacters[index]);
        //}
        //else PhotonNetwork.Destroy(this.gameObject);

        //Debug.Log("Character in index: " + characters[index]);

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

    //void SetTarget(CharacterModel characterTgt)
    //{
    //    characterTarget = characterTgt;
    //    photonView.RPC("UpdateTarget", RpcTarget.Others, characterTgt.photonView.ViewID);
    //}

    //[PunRPC]
    //void UpdateTarget(int id)
    //{
    //    PhotonView view = PhotonView.Find(id);
    //    if (view != null) characterTarget = view.gameObject.GetComponent<CharacterModel>();
    //    agent.SetDestination(characterTarget.transform.position);
    //}

    private void AttackPlayer()
    {
        if (characters.Length >= 1)
        {
            //enemy does not move
            agent.SetDestination(characters[targetIndex].transform.position);

            transform.LookAt(characters[targetIndex].transform);

            if (!alreadyAttacked)
            {
                photonView.RPC("Shoot", PhotonNetwork.LocalPlayer);


                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
    }
    [PunRPC]
    void Shoot()
    {
        GameObject bullet = PhotonNetwork.Instantiate("BananaBulletEnemy", firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), .5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
