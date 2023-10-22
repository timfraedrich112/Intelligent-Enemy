using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSwordHit : MonoBehaviour
{
    [SerializeField] float swordDmg;
    [SerializeField] string agentTag;
    [SerializeField] string visionConeTag;
    [SerializeField] string wallTag;
    public static Action<float> onSwordHit;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(agentTag))
        {
            onSwordHit?.Invoke(swordDmg);
        }
    }
}