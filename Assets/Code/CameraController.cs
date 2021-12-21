using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController player;
    public CameraController myCamera;
    public float mouseSensitivity = 100f;
    public float animTime;

    private float xRotation = 0f;

    public bool IsAnimating { get; private set; }
    private float time;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (player.IsMain)
        {
            if (!IsAnimating)
            {
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                player.root.Rotate(Vector3.up * mouseX);
            }
            else
            {
                time += Time.deltaTime;

                transform.position = Vector3.Lerp(startPosition, targetPosition, time / animTime);
                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, time / animTime);

                if (time >= animTime)
                    IsAnimating = false;
            }
        }
    }

    public void LerpFromOldPlayer(PlayerController oldPlayer)
    {
        IsAnimating = true;
        time = 0f;

        targetPosition = transform.position;
        targetRotation = transform.rotation;

        Transform oldCamera = oldPlayer.cameraController.transform;
        transform.position = oldCamera.position;
        transform.rotation = oldCamera.rotation;

        startPosition = transform.position;
        startRotation = transform.rotation;
    }
}
