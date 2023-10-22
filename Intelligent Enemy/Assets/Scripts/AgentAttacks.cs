using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;

public class AgentAttacks : MonoBehaviour
{
    [SerializeField] string emptyAnimString;
    [SerializeField] string dashStopTag;
    [SerializeField] float turnSpeed;
    [SerializeField] float waitTime;
    [SerializeField] GameObject target;
    [SerializeField] Material red;
    [SerializeField] Material grey;
    [Header("Bonk")]
    [SerializeField] GameObject bonkObj;
    [SerializeField] string bonkAnimString;
    [SerializeField] float bonkTime;
    [SerializeField] float bonkDist;
    [Header("AOE")]
    [SerializeField] GameObject aoeObj;
    [SerializeField] string aoeAnimString;
    [SerializeField] float aoeTime;
    [SerializeField] float aoeDist;
    [Header("Dash")]
    [SerializeField] GameObject dashObj;
    [SerializeField] Renderer leftEye;
    [SerializeField] Renderer rightEye;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [Header("Jump")]
    [SerializeField] GameObject jumpObj;
    [SerializeField] GameObject jumpShockwave;
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpForward;
    [SerializeField] float jumpTime;
    [SerializeField] float jumpDist;
    [Header("Ranged")]
    [SerializeField] GameObject bulletPoolObj;
    [SerializeField] float bulletVelocity;
    [SerializeField] float rangedTime;

    public enum Attacks { bonk, aoe, dash, jump, ranged };
    public List<Attacks> attackQueue;
    [HideInInspector] public bool isAttacking = false;
    bool isDashing = false;

    AgentBulletObjPool bulletPool;
    AgentMemory memory;
    Rigidbody rig;
    NavMeshAgent nav;
    BehaviorExecutor behaviorExecutor;
    Vector3 startPos;
    Quaternion startRot;

    Animator bonkAnim;
    Animator aoeAnim;
    Animator jumpAnim;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        attackQueue = new List<Attacks>();
        rig = GetComponent<Rigidbody>();
        memory = GetComponent<AgentMemory>();
        nav = GetComponent<NavMeshAgent>();
        bulletPool = bulletPoolObj.GetComponent<AgentBulletObjPool>();
        bonkAnim = bonkObj.GetComponent<Animator>();
        aoeAnim = aoeObj.GetComponent<Animator>();
        jumpAnim = GetComponent<Animator>();
        behaviorExecutor = GetComponent<BehaviorExecutor>();

        //StartCoroutine(nameof(TestAttack));
    }

    IEnumerator TestAttack()
    {
        yield return new WaitForSeconds(5f);
        PerformNextAttack();
        yield return new WaitForSeconds(3f);
        PerformNextAttack(); 
        yield return new WaitForSeconds(3f);
        PerformNextAttack();
        yield return new WaitForSeconds(3f);
        PerformNextAttack();
        yield return new WaitForSeconds(3f);
        PerformNextAttack();
    }

    public void PerformNextAttack()
    {
        if (attackQueue.Count > 0 && !isAttacking)
        {
            if (attackQueue[0] == Attacks.bonk)
                StartCoroutine(nameof(Bonk));
            else if (attackQueue[0] == Attacks.aoe)
                StartCoroutine(nameof(Aoe));
            else if (attackQueue[0] == Attacks.dash)
                StartCoroutine(nameof(Dash));
            else if (attackQueue[0] == Attacks.jump)
                StartCoroutine(nameof(Jump));
            else if (attackQueue[0] == Attacks.ranged)
                StartCoroutine(nameof(Ranged));
            attackQueue.Remove(attackQueue[0]);
        }
    }

    IEnumerator Bonk()
    {
        memory.bonkCount++;
        isAttacking = true;
        bonkObj.SetActive(true);
        rig.velocity = Vector3.zero;

        transform.LookAt(target.transform);

        bonkAnim.SetTrigger(bonkAnimString);

        yield return new WaitForSeconds(bonkTime);

        bonkAnim.SetTrigger(emptyAnimString);

        bonkObj.SetActive(false);

        yield return new WaitForSeconds(waitTime);
        isAttacking = false;
    }

    IEnumerator Aoe()
    {
        memory.aoeCount++;
        isAttacking = true;
        aoeObj.SetActive(true);
        rig.velocity = Vector3.zero;

        aoeAnim.SetTrigger(aoeAnimString);

        yield return new WaitForSeconds(aoeTime);

        aoeAnim.SetTrigger(emptyAnimString);

        aoeObj.SetActive(false);

        yield return new WaitForSeconds(waitTime);
        isAttacking = false;
    }

    IEnumerator Dash()
    {
        memory.dashCount++;
        isDashing = true;
        isAttacking = true;

        leftEye.material = red;
        rightEye.material = red;
        yield return new WaitForSeconds(waitTime);

        dashObj.SetActive(true);
        rig.velocity = Vector3.zero;

        transform.LookAt(target.transform);

        rig.AddForce(transform.forward * dashSpeed);

        yield return new WaitForSeconds(dashTime);

        rig.velocity = Vector3.zero;
        dashObj.SetActive(false);
        isDashing = false;
        leftEye.material = grey;
        rightEye.material = grey;

        yield return new WaitForSeconds(waitTime);
        isAttacking = false;
    }

    IEnumerator Jump()
    {
        memory.jumpCount++;
        isAttacking = true;
        rig.velocity = Vector3.zero;

        transform.LookAt(target.transform);

        nav.enabled = false;
        rig.velocity = Vector3.zero;
        rig.freezeRotation = true;
        rig.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
        rig.AddForce(transform.forward * jumpForward, ForceMode.Impulse);

        yield return new WaitForSeconds(jumpTime);

        nav.enabled = true;
        rig.velocity = Vector3.zero;
        isAttacking = false;

        jumpShockwave.SetActive(true);
        jumpObj.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        jumpShockwave.SetActive(false);
        jumpObj.SetActive(false);
    }

    IEnumerator Ranged()
    {
        memory.rangedCount++;
        isAttacking = true;
        rig.velocity = Vector3.zero;

        GameObject bullet = bulletPool.SpawnPooledObject();
        Rigidbody bulletRig = bullet.GetComponent<Rigidbody>();

        bullet.transform.LookAt(target.transform);

        bulletRig.velocity = Vector3.zero;
        bulletRig.AddForce(bullet.transform.forward * bulletVelocity, ForceMode.Impulse);

        yield return new WaitForSeconds(rangedTime);
        isAttacking = false;
    }

    public bool RangeToAttack()
    {
        if (attackQueue.Count > 0)
        {
            if (attackQueue[0] == Attacks.bonk)
            {
                if (memory.distBetween > bonkDist)
                {
                    return true;
                }
            }
            else if (attackQueue[0] == Attacks.aoe)
            {
                if (memory.distBetween > aoeDist)
                {
                    return true;
                }
            }
            else if (attackQueue[0] == Attacks.jump)
            {
                if (memory.distBetween > jumpDist)
                {
                    return true;
                }
            }
            else
                return false;
        }
        return false;
    }

    public void GoToTargetLocation()
    {
        if (attackQueue.Count > 0)
        {
            if (attackQueue[0] == Attacks.bonk)
            {
                if (memory.distBetween > bonkDist)
                {
                    nav.destination = memory.player.gameObject.transform.position;
                }
            }
            else if (attackQueue[0] == Attacks.aoe)
            {
                if (memory.distBetween > aoeDist)
                {
                    nav.destination = memory.player.gameObject.transform.position;
                }
            }
            else if (attackQueue[0] == Attacks.jump)
            {
                if (memory.distBetween > jumpDist)
                {
                    nav.destination = memory.player.gameObject.transform.position;
                }
            }
            else
                nav.destination = transform.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(dashStopTag))
        {
            if (isDashing)
            {
                rig.velocity = Vector3.zero;
            }
        }
    }

    void OnEnable()
    {
        AgentHealth.gameEnd += EndGame;
        PlayerHealth.gameEnd += EndGame;
    }

    void EndGame()
    {
        behaviorExecutor.enabled = false;
    }

    void OnDisable()
    {
        AgentHealth.gameEnd -= EndGame;
        PlayerHealth.gameEnd -= EndGame;
    }
}