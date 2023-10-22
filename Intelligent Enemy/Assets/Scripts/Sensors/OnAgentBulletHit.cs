using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAgentBulletHit : MonoBehaviour
{
    [SerializeField] float bulletDmg;
    [SerializeField] string playerTag;
    [SerializeField] string shieldTag;
    [SerializeField] string wallTag;
    public static Action<float> onAgentBulletHit;

    bool blocked = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag) && !blocked)
        {
            onAgentBulletHit?.Invoke(bulletDmg);
            gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag(shieldTag) || other.gameObject.CompareTag(wallTag))
        {
            gameObject.SetActive(false);
        }
    }
}