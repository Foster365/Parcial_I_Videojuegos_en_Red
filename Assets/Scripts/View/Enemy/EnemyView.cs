using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : EntityView
{
    public void HandlePunchingAnim(bool _isPunching)
    {
        anim.SetBool(TagManager.PUNCHING_ANIMATION_TAG, _isPunching);
    }
}
