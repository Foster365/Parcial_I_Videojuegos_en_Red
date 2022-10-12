using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class PlayerNickname : MonoBehaviour
{
    public TextMeshProUGUI nickNameUI;
    public Vector3 offset;
    Transform target;
    Camera camera;
    private void Awake()
    {
        camera = Camera.main;
    }
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    public void SetName(string nick)
    {
        nickNameUI.text = nick;
    }

    private void Update()
    {
        if (target != null)
        {
            var pos = camera.WorldToScreenPoint(target.position + offset);
            transform.position = pos;
        }
    }
}