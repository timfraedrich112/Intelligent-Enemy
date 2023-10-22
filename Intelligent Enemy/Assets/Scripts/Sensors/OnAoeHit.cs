using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAoeHit : MonoBehaviour
{
    [SerializeField] float aoeDmg;
    [SerializeField] string playerTag;
    [SerializeField] string shieldTag;
    public static Action<float> onAoeHit;

    bool blocked = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(shieldTag))
        {
            blocked = true;
        }
        if (other.gameObject.CompareTag(playerTag) && !blocked)
        {
            onAoeHit?.Invoke(aoeDmg);
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