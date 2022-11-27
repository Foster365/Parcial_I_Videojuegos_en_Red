using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static LevelsManager;

public class CharacterModel : MonoBehaviourPun
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private bool rotateTowardsMouse;

    public float bulletForce = 20f;

    bool hasPlayerAlreadySpawned = false;
    HealthManager healthMgr;
    private CharacterController input;
    public Transform firePoint;

    private Camera camera;
    public bool HasPlayerAlreadySpawned { get => hasPlayerAlreadySpawned; set => hasPlayerAlreadySpawned = value; }
    public HealthManager HealthMgr { get => healthMgr; set => healthMgr = value; }

    private void Awake()
    {
        if (!photonView.IsMine) Destroy(this);
        camera = Camera.main.gameObject.GetComponent<Camera>();
        input = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        healthMgr = GetComponent<HealthManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            var targetVector = new Vector3(input.InputVector.x, 0, input.InputVector.y);

            var movementVector = MoveTowardsTarget(targetVector);

            if (!rotateTowardsMouse) RotateTowardMovementVector(movementVector);
            else RotateTowardMouseVector();
        }
    }

    private void RotateTowardMouseVector()
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

    private void RotateTowardMovementVector(Vector3 movementVector)
    {
        if (movementVector.magnitude == 0) return;
        var rotation = Quaternion.LookRotation(movementVector);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
    }

    private Vector3 MoveTowardsTarget(Vector3 targetVector)
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
        GameObject bullet = PhotonNetwork.Instantiate("BananaBullet", firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
    }
}
