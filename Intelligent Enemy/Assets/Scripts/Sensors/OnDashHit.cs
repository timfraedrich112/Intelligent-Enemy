using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDashHit : MonoBehaviour
{
    [SerializeField] float dashDmg;
    [SerializeField] string playerTag;
    [SerializeField] string shieldTag;
    public static Action<float> onDashHit;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            onDashHit?.Invoke(dashDmg);
        } 
    }
}