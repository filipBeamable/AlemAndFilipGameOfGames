using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform root;
    public CameraController cameraController;
    public CharacterController characterController;
    public float moveSpeed;
    public float jumpHeight;
    public float gravity = 9.8f;

    private Vector3 gravityVelocity;

    private void Update()
    {
        bool groundedPlayer = characterController.isGrounded;
        if (groundedPlayer && gravityVelocity.y < 0)
            gravityVelocity.y = 0f;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool jumpDown = Input.GetButtonDown("Jump");

        Vector3 moveVelocity = Vector3.zero;
        if (Mathf.Abs(horizontal) > 0.001f)
        {
            Vector3 right = cameraController.transform.right;
            right.y = 0;
            right.Normalize();
            moveVelocity += right * horizontal;
        }
        if (Mathf.Abs(vertical) > 0.001f)
        {
            Vector3 forward = cameraController.transform.forward;
            forward.y = 0;
            forward.Normalize();
            moveVelocity += forward * vertical;
        }
        characterController.Move(moveVelocity.normalized * moveSpeed * Time.deltaTime);

        if (jumpDown)
        {
            Debug.LogError("HERE");
            if (groundedPlayer)
            {
                gravityVelocity.y += Mathf.Sqrt(jumpHeight * 3.0f * gravity);
            }
        }

        gravityVelocity.y -= gravity * Time.deltaTime;
        characterController.Move(gravityVelocity * Time.deltaTime);
    }
}
