using BBUnity.Actions;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField]
    Transform orientation;
    [SerializeField]
    GameObject player;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 lookDir = player.transform.position - new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        orientation.forward = lookDir.normalized;
    }
}