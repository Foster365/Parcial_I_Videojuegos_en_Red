using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : EntityView
{
    [SerializeField] GameObject playerNickPrefab;
    PlayerNickname playerNick;
    [SerializeField] bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        var gameCanvas = GameObject.Find("Canvas");
        playerNick = GameObject.Instantiate(playerNickPrefab, gameCanvas.transform).gameObject.GetComponent<PlayerNickname>();
        playerNick.SetTarget(transform);

        if (photonView.IsMine)
        {
            var nick = photonView.Owner.NickName;
            UpdateNick(nick);
        }
        else photonView.RPC("RequestNick", photonView.Owner, PhotonNetwork.LocalPlayer);
    }
    private void Update()
    {
        if (isDead) OnDestroyNick();
        //else if (Input.GetKeyDown(KeyCode.Q)) anim.SetBool("Punch", true);
    }

    [PunRPC]
    void UpdateNick(string nickName)
    {
        if (playerNick != null) playerNick.SetName(nickName);
    }

    [PunRPC]
    void RequestNick(Player player)
    {
        photonView.RPC("UpdateNick", player, photonView.Owner.NickName);
    }
    void OnDestroyNick()
    {
        PhotonNetwork.Destroy(playerNick.gameObject);
    }
}
