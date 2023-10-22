using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] Transform player;

    void Update()
    {
        Vector3 target = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(target);
    }
}
