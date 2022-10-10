using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class InputHandler : MonoBehaviourPun
{
    public Vector2 InputVector { get; private set; }

    public Vector3 MousePosition { get; private set; }
    void Update()
    {
        if(photonView.IsMine)
        {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");
            InputVector = new Vector2(h, v);

            MousePosition = Input.mousePosition;
        }
    }
}
