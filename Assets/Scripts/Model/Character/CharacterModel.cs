using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static LevelsManager;

public class CharacterModel : MonoBehaviourPun
{
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float rotateSpeed;
    [SerializeField]
    bool rotateTowardsMouse;

    public float bulletForce = 20f;

    bool hasPlayerAlreadySpawned = false;
    HealthManager healthMgr;
    CharacterController input;
    public Transform firePoint;
    bool isShooting = false;

    Camera camera;
    CharacterView charView;

    public bool HasPlayerAlreadySpawned { get => hasPlayerAlreadySpawned; set => hasPlayerAlreadySpawned = value; }
    public HealthManager HealthMgr { get => healthMgr; set => healthMgr = value; }
    public bool RotateTowardsMouse { get => rotateTowardsMouse; set => rotateTowardsMouse = value; }
    public bool IsShooting { get => isShooting; set => isShooting = value; }

    void Awake()
    {
        if (!photonView.IsMine) Destroy(this);
        camera = Camera.main.gameObject.GetComponent<Camera>();
        input = GetComponent<CharacterController>();
        charView = GetComponent<CharacterView>();

    }

    // Start is called before the first frame update
    void Start()
    {
        healthMgr = GetComponent<HealthManager>();
    }

    public void RotateTowardMouseVector()
    {
        if (photonView.IsMine)
        {
            Ray ray = camera.ScreenPointToRay(input.MousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
            {
                var target = hitInfo.point;
                target.y = transform.position.y;
                transform.LookAt(target);
            }
        }
    }

    public void RotateTowardMovementVector(Vector3 movementVector)
    {
        if (movementVector.magnitude == 0) return;
        var rotation = Quaternion.LookRotation(movementVector);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
    }

    public Vector3 MoveTowardsTarget(Vector3 targetVector)
    {
        var speed = moveSpeed * Time.deltaTime;

        targetVector = Quaternion.Euler(0, camera.gameObject.transform.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + (targetVector * speed);
        transform.position = targetPosition;
        return targetVector;
    }
    [PunRPC]
    void Shoot()
    {

        charView.HandleShootAnim(true);
        GameObject bullet = PhotonNetwork.Instantiate("BananaBullet", firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
        StartCoroutine(WaitToDisableShootAnim());
    }
    IEnumerator WaitToDisableShootAnim()
    {
        yield return new WaitForSeconds(.1f);
        charView.HandleShootAnim(false);
    }
}
