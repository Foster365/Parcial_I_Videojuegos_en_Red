using Photon.Pun;
using Photon.Realtime;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowUp : MonoBehaviourPun // TODO # Note: Este script contiene bugs. Deber�a 1) Solucionarse el bug. 2) Eliminarse de Unity.
{

    float damping = 12f, height = 13, offset = 0, viewDistance = 3;
    Vector3 center;
    //   var Damping = 12.0;
    //   var Player : Transform;
    //var Height : float = 13.0;
    //var Offset : float = 0.0;

    //private var Center : Vector3;
    //var ViewDistance : float = 3.0;
    void Update()
    {
        //if (photonView.IsMine)
        //{
        var mousePos = Input.mousePosition;
        mousePos.z = viewDistance;
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 playerPosition = transform.position;

        center = new Vector3((playerPosition.x + cursorPosition.x) / 2, playerPosition.y, (playerPosition.z + cursorPosition.z) / 2);

        transform.position = Vector3.Lerp(transform.position, center + new Vector3(0, height, offset).normalized, Time.deltaTime * damping); // TODO BUG: Esta l�nea de c�digo produce que el player salte constantemente, y no hace que la c�mara siga al player al moverse.
    }


    private void OnDrawGizmos()
    {
        if (photonView.IsMine)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(center, 1f);
        }
    }
}
