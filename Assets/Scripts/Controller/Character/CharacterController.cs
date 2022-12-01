using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviourPun
{
    CharacterModel charModel;
    CharacterView charView;
    float cooldownShoot;
    public Vector2 InputVector { get; private set; }

    public Vector3 MousePosition { get; private set; }

    private void Awake()
    {
        if (!photonView.IsMine) Destroy(this);
        charModel = GetComponent<CharacterModel>();
        charView = GetComponent<CharacterView>();
    }
    private void Start()
    {
        cooldownShoot = 0;
    }
    void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        InputVector = new Vector2(h, v);

        MousePosition = Input.mousePosition;

        var dir = new Vector3(h, 0, v);

        var movementVector = charModel.MoveTowardsTarget(dir);
        HandleAnim(dir);
        HandleMovement(movementVector);
        HandleShootingInput();
    }

    public void HandleAnim(Vector3 _dir)
    {
        if (_dir != Vector3.zero) charView.HandleRunAnim(true);
        else charView.HandleRunAnim(false);
    }

    void HandleShootingInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            photonView.RPC("Shoot", PhotonNetwork.LocalPlayer);
        }
        //else charView.HandleShootAnim(false);
    }

    public void HandleMovement(Vector3 _movementVector)
    {

        if (!charModel.RotateTowardsMouse) charModel.RotateTowardMovementVector(_movementVector);
        else charModel.RotateTowardMouseVector();

    }
}
