using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyModel : MonoBehaviourPun // TODO # Note: Se modificará la lógica en este script con la nueva lógica de los enemies.
{

    [Header("Basic Variables")]
    [SerializeField] bool isDistanceEnemy;

    [Header("Movement Variables")]
    [SerializeField] float movementSpeed;
    [SerializeField] float rotSpeed;

    [Header("Attack Variables")]
    [SerializeField] float attackRange;
    [SerializeField] float maxAttackTimer;
    [SerializeField] Transform firePoint;

    float attackTimer;
    bool canMove;
    public float bulletForce;

    Rigidbody rb;
    HealthManager healthMgr;
    Animator anim;
    EnemyView enemView;

    CharacterModel target;

    public HealthManager HealthMgr { get => healthMgr; set => healthMgr = value; }
    public CharacterModel Target { get => target; set => target = value; }
    public float AttackRange { get => attackRange; }
    public bool CanMove { get => canMove; set => canMove = value; }

    // Start is called before the first frame update
    void Awake()
    {
        if (!photonView.IsMine) Destroy(this);
        rb = GetComponent<Rigidbody>();
        healthMgr = GetComponent<HealthManager>();
        anim = GetComponent<Animator>();
        enemView = GetComponent<EnemyView>();
    }

    private void Start()
    {
        canMove = false;
        attackTimer = 0;
        bulletForce = 20f;
    }

    #region Targetting_Methods
    public void SetTarget(CharacterModel _target)
    {
        //if (!_isInCooldown)
        //{
        target = _target;
        photonView.RPC("UpdateTarget", RpcTarget.Others, target.photonView.ViewID);
        //}
    }
    public void SetRandomTarget()
    {
        if (photonView.IsMine)
        {
            GameObject[] characters = GameObject.FindGameObjectsWithTag(TagManager.PLAYER_TAG);//FindObjectsOfType<CharacterModel>();
            Debug.Log("Characters length is: " + characters.Length);
            if (characters.Length > 0)
            {
                List<CharacterModel> list = new List<CharacterModel>();
                for (int i = 0; i < characters.Length; i++)
                {
                    list.Add(characters[i].gameObject.GetComponent<CharacterModel>());
                }
                int index = Random.Range(0, list.Count);
                SetTarget(list[index]);
            }
        }
    }
    [PunRPC]
    public void UpdateTarget(int id)
    {
        PhotonView view = PhotonView.Find(id);
        if (view != null)
        {
            target = view.gameObject.GetComponent<CharacterModel>();
        }
    }
    #endregion

    #region Seeking_Methods
    public void SeekCharacter()
    {
        Vector3 dir = target.transform.position - transform.position;
        canMove = true;
        if (canMove)
        {
            Move(dir.normalized);
            LookDir(dir);
        }
    }

    public void Move(Vector3 dir)
    {
        dir *= movementSpeed;
        dir.y = rb.velocity.y;
        rb.velocity = dir;
    }

    #endregion

    public void LookDir(Vector3 dir)
    {
        transform.forward = Vector3.Lerp(transform.forward, dir, rotSpeed * Time.deltaTime);
    }

    public void AttackCharacter()
    {
        canMove = false;

        if (isDistanceEnemy) HandleShooting();
        else HandleMeleeAttack();
    }

    #region Distance_Attack_Methods
    public void HandleShooting()
    {
        Vector3 dir = target.transform.position - transform.position;
        LookDir(dir);
        attackTimer += Time.deltaTime;
        if (attackTimer >= maxAttackTimer)
        {
            LookDir(dir);
            Shoot();
            attackTimer = 0;
            //enemView.HandleShootAnim(false);
        }
    }

    [PunRPC]
    void Shoot()
    {

        //enemView.HandleShootAnim(true);
        GameObject bullet = PhotonNetwork.Instantiate("EnemyBullet", firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
        //StartCoroutine(WaitToDisableAnim());
    }

    public IEnumerator WaitToDisableAnim()
    {
        yield return new WaitForSeconds(.5f);
    }
    #endregion

    #region Melee_Attack_Methods
    public void HandleMeleeAttack()
    {
        enemView.HandlePunchingAnim(true);
        attackTimer += Time.deltaTime;
        if (attackTimer >= maxAttackTimer)
        {
            enemView.HandlePunchingAnim(false);
            attackTimer = 0;
        }
    }
    #endregion
}
