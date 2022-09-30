using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboScript : MonoBehaviour
{
    private Animator anim;
    public float cooldownTime = 1f;
    private float nextFireTime = 0f;
    public static int numbOfClicks = 0;
    float lastClickedTime = 0f;
    float maxComboDelay = 1;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit 1"))
        {
            anim.SetBool("hit 1", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit 2"))
        {
            anim.SetBool("hit 2", false);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit 3"))
        {
            anim.SetBool("hit 3", false);
            numbOfClicks = 0;
        }

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            numbOfClicks = 0;
        }

        if (Time.time > nextFireTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnClick();
            }
        }
    }

    void OnClick()
    {
        lastClickedTime = Time.time;
        numbOfClicks++;
        if(numbOfClicks == 1)
        {
            anim.SetBool("hit 1", true);
        }
        numbOfClicks = Mathf.Clamp(numbOfClicks, 0, 3);

        if (numbOfClicks >= 2 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit 1"))
        {
            anim.SetBool("hit 1", false);
            anim.SetBool("hit 2", true);
        }
        if (numbOfClicks >= 3 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit 2"))
        {
            anim.SetBool("hit 2", false);
            anim.SetBool("hit 3", true);
        }
    }
}
