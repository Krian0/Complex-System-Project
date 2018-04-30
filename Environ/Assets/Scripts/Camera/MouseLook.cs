using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 4;

    public float camRotationMinClamp = -60;
    public float camRotationMaxClamp = 80;

    [Space(16)]

    public GameObject player;

    [Space(16)]

    public float yaw;
    public float pitch;


    private void Start()
    {
        player = transform.parent.gameObject;
    }

    private void Update()
    {
        yaw = Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, camRotationMinClamp, camRotationMaxClamp);

        if (GetComponentInParent<PlayerController>().canMove)
        {
            player.transform.Rotate(0, yaw, 0);
            transform.localRotation = Quaternion.Euler(pitch, player.transform.rotation.y, player.transform.rotation.z);
        }
    }
}
