using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnJumpHit : MonoBehaviour
{
    [SerializeField] float jumpDmg;
    [SerializeField] string playerTag;
    [SerializeField] string shieldTag;
    public static Action<float> onJumpHit;

    bool blocked = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(shieldTag))
        {
            blocked = true;
        }
        if (other.gameObject.CompareTag(playerTag) && !blocked)
        {
            onJumpHit?.Invoke(jumpDmg);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(shieldTag))
        {
            blocked = false;
        }
    }
}