using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviourPun
{
    [SerializeField] GameObject playerNickPrefab;
    PlayerNickname playerNick;
    [SerializeField] bool isDead = false;
    public Animator anim;


    private void Awake()
    {
        if (photonView.IsMine) anim = GetComponent<Animator>();
    }
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

    public void HandleRunAnim(bool isRunning)
    {
        anim.SetBool(TagManager.MOVING_ANIMATION_TAG, isRunning);
    }
    public void HandleShootAnim(bool isShooting)
    {
        anim.SetBool(TagManager.SHOOTING_ANIMATION_TAG, isShooting);
    }
    public void HandleHitAnim(bool isHit)
    {
        anim.SetBool(TagManager.HIT_ANIMATION_TAG, isHit);
    }
    public void HandleDeathAnim(bool isDead)
    {
        anim.SetBool(TagManager.DEATH_ANIMATION_TAG, isDead);
    }
}
