using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviourPun
{
    CharacterModel charModel;
    public Vector2 InputVector { get; private set; }

    public Vector3 MousePosition { get; private set; }

    private void Awake()
    {
        if (!photonView.IsMine) Destroy(this);
        charModel = GetComponent<CharacterModel>();
    }
    void Update()
    {

        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        InputVector = new Vector2(h, v);

        MousePosition = Input.mousePosition;

        if (Input.GetButtonDown("Fire1")) photonView.RPC("Shoot", PhotonNetwork.LocalPlayer);
    }
}
