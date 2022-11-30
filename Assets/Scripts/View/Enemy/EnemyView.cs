using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviourPun
{

    public Animator anim;

    private void Awake()
    {
        if (photonView.IsMine) anim = GetComponent<Animator>();
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
