using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollidersController : MonoBehaviour
{
    [SerializeField] GameObject attacker, leftHandCollider, rightHandCollider;

    void LeftArmAttackOn()
    {
        leftHandCollider.SetActive(true);
    }

    void LeftArmAttackOff()
    {
        if (leftHandCollider.activeInHierarchy) leftHandCollider.SetActive(false);
    }

    void RightArmAttackOn()
    {
        rightHandCollider.SetActive(true);
    }

    void RightArmAttackOff()
    {
        if (rightHandCollider.activeInHierarchy) rightHandCollider.SetActive(false);
    }

}
