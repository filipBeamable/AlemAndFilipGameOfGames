using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform root;
    public CameraController cameraController;
    public Rigidbody myRigidbody;
    public float moveSpeed;

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) > 0.001f)
        {
            Vector3 right = cameraController.transform.right;
            right.y = 0;
            myRigidbody.MovePosition(myRigidbody.position + (right * horizontal * moveSpeed * Time.deltaTime));
        }
        if (Mathf.Abs(vertical) > 0.001f)
        {
            Vector3 forward = cameraController.transform.forward;
            forward.y = 0;
            myRigidbody.MovePosition(myRigidbody.position + (forward * vertical * moveSpeed * Time.deltaTime));
        }
    }
}
