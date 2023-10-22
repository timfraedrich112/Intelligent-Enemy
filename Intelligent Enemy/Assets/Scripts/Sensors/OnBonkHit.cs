using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBonkHit : MonoBehaviour
{
    [SerializeField] float bonkDmg;
    [SerializeField] string playerTag;
    [SerializeField] string shieldTag;
    public static Action<float> onBonkHit;

    bool blocked = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(shieldTag))
        {
            blocked = true;
        }
        if (other.gameObject.CompareTag(playerTag) && !blocked)
        {
            onBonkHit?.Invoke(bonkDmg);
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