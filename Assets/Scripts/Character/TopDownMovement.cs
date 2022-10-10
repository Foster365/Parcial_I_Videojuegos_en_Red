using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class TopDownMovement : MonoBehaviourPun
{
    private InputHandler input;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private bool rotateTowardsMouse;

    private Camera camera;
    private void Awake()
    {
        if (!photonView.IsMine) Destroy(this);
        camera = GameObject.FindWithTag(TagManager.MAIN_CAMERA_TAG).gameObject.GetComponent<Camera>();
        input = GetComponent<InputHandler>();
    }

    //void OnTriggerEnter(Collider coll)
    //{
    //   if (coll.CompareTag("Enemy"))
    //    {
    //        print("dmg");
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            var targetVector = new Vector3(input.InputVector.x, 0, input.InputVector.y);

            var movementVector = MoveTowardsTarget(targetVector);

            if (!rotateTowardsMouse) RotateTowardMovementVector(movementVector);
            else RotateTowardMouseVector();
        }
    }

    private void RotateTowardMouseVector()
    {
        Debug.Log("Pan con pimienta");
        Ray ray = camera.ScreenPointToRay(input.MousePosition);

        if(Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            var target = hitInfo.point;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
    }

    private void RotateTowardMovementVector(Vector3 movementVector)
    {
        Debug.Log("Clavicula de papas");
        if(movementVector.magnitude == 0) return;
        var rotation = Quaternion.LookRotation(movementVector);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
    }

    private Vector3 MoveTowardsTarget(Vector3 targetVector)
    {
        var speed = moveSpeed * Time.deltaTime;

        targetVector = Quaternion.Euler(0, camera.gameObject.transform.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
        return targetVector;
    }
}
