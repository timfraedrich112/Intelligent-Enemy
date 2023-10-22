using BBUnity.Actions;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform orientation;
    [SerializeField] float groundDrag;
    [Header("Movement")]
    [SerializeField] float moveForce;
    [SerializeField] float shieldMoveForce;
    [SerializeField] float airMoveForce;
    [SerializeField] float topSpeed;
    [Header("Jump")]
    [SerializeField] float jumpForce;
    [SerializeField] float jumpVelFalloff;
    [SerializeField] float fallMultiplier;
    [SerializeField] float topFallSpeed;
    [Header("Dash")]
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [SerializeField] float dashCooldown;
    [Header("Sword")]
    [SerializeField] GameObject swordObj;
    [SerializeField] float swingTime;
    [SerializeField] string swordAnimString;
    [SerializeField] string emptyAnimString;
    [Header("Gun")]
    [SerializeField] GameObject gunObj;
    [SerializeField] GameObject bulletPoolObj;
    [SerializeField] float bulletVelocity;
    [SerializeField] float shootTime;
    [Header("Shield")]
    [SerializeField] GameObject shieldObj;

    public enum playerEquip { Shield, Sword, Gun};
    playerEquip equip;
    playerEquip temp;

    public enum playerState { Moving, Attacking, Defending};
    [HideInInspector] public playerState state;

    Vector2 moveDir, dashDir;

    bool isGrounded = true, isDashing = false, canDash = true;
    bool isAttacking = false, isShielding = false;

    Rigidbody rig;
    PlayerBulletObjPool bulletPool;
    Animator swordAnim;
    Vector3 startPos;
    Quaternion startRot;

    void Start()
    {
        bulletPool = bulletPoolObj.GetComponent<PlayerBulletObjPool>();
        Physics.gravity = new(0f, -20f, 0f);
        rig = GetComponent<Rigidbody>();
        swordAnim = swordObj.GetComponent<Animator>();
        rig.freezeRotation = true;
        state = playerState.Moving;
        equip = playerEquip.Sword;
        startPos = transform.position;
        startRot = transform.rotation;
    }

    void Update()
    {
        if (equip == playerEquip.Sword)
        {
            swordObj.SetActive(true);
            gunObj.SetActive(false);
            shieldObj.SetActive(false);
        }
        if (equip == playerEquip.Gun)
        {
            swordObj.SetActive(false);
            gunObj.SetActive(true);
            shieldObj.SetActive(false);
        }
        if (equip == playerEquip.Shield)
        {
            swordObj.SetActive(false);
            gunObj.SetActive(false);
            shieldObj.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        transform.rotation = orientation.rotation;

        if (!isDashing)
            MovePlayer();

        //push player down at peak of jump
        if (rig.velocity.y < jumpVelFalloff)
            rig.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;

        //enforce terminal velocity
        if (rig.velocity.y < topFallSpeed)
            rig.velocity = new(rig.velocity.x, topFallSpeed, rig.velocity.z);

        //set drag based on grounded or not
        if (isGrounded)
            rig.drag = groundDrag;
        else
            rig.drag = 0f;
    }

    void MovePlayer()
    {
        //enforce max move speed
        Vector3 lateralVel = new(rig.velocity.x, 0f, rig.velocity.z);
        if (lateralVel.magnitude > topSpeed)
        {
            Vector3 newVel = lateralVel.normalized * topSpeed;
            rig.velocity = new(newVel.x, rig.velocity.y, newVel.z);
            return;
        }

        //move player, speed based on in air or not
        if (moveDir != Vector2.zero)
        {
            Vector3 moveDirection = orientation.forward * moveDir.y + orientation.right * moveDir.x;
            if (isGrounded && !isShielding)
            {
                rig.AddForce(moveDirection.normalized * moveForce, ForceMode.Force);
            }
            else if (isShielding)
            {
                rig.AddForce(moveDirection.normalized * shieldMoveForce, ForceMode.Force);
            }
            else if (!isGrounded)
            {
                rig.AddForce(moveDirection.normalized * airMoveForce, ForceMode.Force);
            }
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        float originalDrag = rig.drag;
        rig.drag = 0f;

        Vector3 moveDirection;
        if (dashDir == Vector2.zero)
            moveDirection = orientation.forward;
        else
            moveDirection = orientation.forward * dashDir.y + orientation.right * dashDir.x;
        rig.AddForce(moveDirection * dashSpeed, ForceMode.Impulse);

        yield return new WaitForSeconds(dashTime);

        rig.drag = originalDrag;

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    IEnumerator SwingSword()
    {
        isAttacking = true;
        state = playerState.Attacking;

        //swing the sword
        swordAnim.SetTrigger(swordAnimString);
        yield return new WaitForSeconds(swingTime);
        swordAnim.SetTrigger(emptyAnimString);

        isAttacking = false;
        state = playerState.Moving;
    }

    IEnumerator ShootGun()
    {
        isAttacking = true;
        state = playerState.Attacking;

        //shoot the gun
        GameObject bullet = bulletPool.SpawnPooledObject();
        Rigidbody bulletRig = bullet.GetComponent<Rigidbody>();
        bullet.transform.forward = orientation.forward;
        bulletRig.velocity = Vector3.zero;
        bulletRig.AddForce(bullet.transform.forward * bulletVelocity, ForceMode.Impulse);

        yield return new WaitForSeconds(shootTime);

        isAttacking = false;
        state = playerState.Moving;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
            canDash = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }

    void OnEnable()
    {
        InputManager.onMove += OnMove;
        InputManager.onJump += OnJump;
        InputManager.onDash += OnDash;
        InputManager.onAttack += OnAttack;
        InputManager.onSwap += OnSwap;
        InputManager.onShield += OnShield;
    }

    void OnMove(Vector2 dir)
    {
        moveDir = dir;
        dashDir = dir;
    }

    void OnJump()
    {
        if (isGrounded && !isAttacking && !isShielding)
        {
            rig.velocity = Vector3.zero;
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canDash = true;
        }
    }

    void OnDash()
    {
        if (!isDashing && canDash && !isAttacking && !isShielding)
        {
            StartCoroutine(nameof(Dash));
        }
    }

    void OnAttack()
    {
        if (!isDashing && !isAttacking && !isShielding)
        {
            if (equip == playerEquip.Sword)
                StartCoroutine(nameof(SwingSword));
            else if (equip == playerEquip.Gun)
                StartCoroutine(nameof(ShootGun));
        }
    }

    void OnSwap()
    {
        if (!isDashing && !isAttacking && !isShielding)
        {
            if (equip == playerEquip.Sword)
            {
                equip = playerEquip.Gun;
            }
            else if (equip == playerEquip.Gun)
            {
                equip = playerEquip.Sword;
            }
        }
    }

    void OnShield(float value)
    {
        if (value == 1)
        {
            if (isGrounded && !isDashing && !isAttacking)
            {
                isShielding = true;
                state = playerState.Defending;
                temp = equip;
                equip = playerEquip.Shield;
            }
        }
        else if (value == 0)
        {
            if (isShielding)
            {
                isShielding = false;
                state = playerState.Moving;
                equip = temp;
            }
        }
    }

    void OnDisable()
    {
        InputManager.onMove -= OnMove;
        InputManager.onJump -= OnJump;
        InputManager.onDash -= OnDash;
        InputManager.onAttack -= OnAttack;
        InputManager.onSwap -= OnSwap;
        InputManager.onShield -= OnShield;
    }
}
