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
    public LayerMask groundedCheckLayerMask;

    [Header("Shooting")]
    public Transform shootPoint;
    public GameObject shootLinePrefab;

    private Vector3 gravityVelocity;

    public bool IsMain { get; set; }

    private void Update()
    {
        bool groundedPlayer = characterController.isGrounded;
        if (!groundedPlayer)
        {
            if (Physics.Raycast(new Ray(transform.position + new Vector3(0, 0.05f), Vector3.down), out RaycastHit hitInfo, 0.3f, groundedCheckLayerMask))
                groundedPlayer = true;
        }

        if (groundedPlayer && gravityVelocity.y < 0)
            gravityVelocity.y = 0f;

        if (IsMain && !cameraController.IsAnimating)
        {
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

            if (jumpDown && groundedPlayer)
                    gravityVelocity.y += Mathf.Sqrt(jumpHeight * 3.0f * gravity);

            if (Input.GetButtonDown("Fire1"))
            {
                Vector3 endPoint = shootPoint.position + (shootPoint.forward * 100);
                if (Physics.Raycast(new Ray(shootPoint.position, shootPoint.forward), out RaycastHit hitInfo))
                    endPoint = hitInfo.point;

                Instantiate(shootLinePrefab).GetComponent<ShootLine>().Init(shootPoint.position, endPoint);
            }
        }

        gravityVelocity.y -= gravity * Time.deltaTime;
        characterController.Move(gravityVelocity * Time.deltaTime);
    }

    public void SetIsMain(bool isMain)
    {
        IsMain = isMain;
        cameraController.gameObject.SetActive(isMain);
    }
}
