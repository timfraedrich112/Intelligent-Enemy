using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerBulletHit : MonoBehaviour
{
    [SerializeField] float bulletDmg;
    [SerializeField] string agentTag;
    [SerializeField] string wallTag;
    public static Action<float> onPlayerBulletHit;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(agentTag))
        {
            onPlayerBulletHit?.Invoke(bulletDmg);
            gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag(wallTag))
        {
            gameObject.SetActive(false);
        }
    }
}