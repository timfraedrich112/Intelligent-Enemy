using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMemory : MonoBehaviour
{
    [SerializeField] GameObject playerObj;
    [SerializeField] public float meleeDist;
    [SerializeField] public float frequentShield;
    [SerializeField] public float frequentAttack;
    [SerializeField] public float accuracyThreshold;

    //[Header("Memory")]
    [HideInInspector] public float distBetween;
    [HideInInspector] public int bonkCount = 0, bonkHit, bonkAccuracy = 1;
    [HideInInspector] public int aoeCount = 0, aoeHit, aoeAccuracy = 1;
    [HideInInspector] public int dashCount = 0, dashHit, dashAccuracy = 1;
    [HideInInspector] public int jumpCount = 0, jumpHit, jumpAccuracy = 1;
    [HideInInspector] public int rangedCount = 0, rangedHit, rangedAccuracy = 1;
    [HideInInspector] public float timeDef, timeAtk, timeMov, timeTotal;
    [HideInInspector] public float rangeDmgTaken, meleeDmgTaken;
    [HideInInspector] public PlayerController player;

    void Start()
    {
        player = playerObj.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (playerObj != null)
        {
            timeTotal += Time.deltaTime;
            if (player.state == PlayerController.playerState.Attacking)
                timeAtk += Time.deltaTime;
            else if (player.state == PlayerController.playerState.Defending)
                timeDef += Time.deltaTime;
            else if (player.state == PlayerController.playerState.Moving)
                timeMov += Time.deltaTime;

            distBetween = Vector3.Distance(transform.position, playerObj.gameObject.transform.position);
        }
    }

    void OnEnable()
    {
        OnBonkHit.onBonkHit += OnBonkLanded;
        OnAoeHit.onAoeHit += OnAoeLanded;
        OnDashHit.onDashHit += OnDashLanded;
        OnJumpHit.onJumpHit += OnJumpLanded;
        OnAgentBulletHit.onAgentBulletHit += OnRangedLanded;
        OnSwordHit.onSwordHit += OnMeleeTaken;
        OnPlayerBulletHit.onPlayerBulletHit += OnRangedTaken;
    }

    void OnBonkLanded(float value)
    {
        bonkHit++;
        bonkAccuracy = bonkHit / bonkCount;
    }

    void OnAoeLanded(float value)
    {
        aoeHit++;
        aoeAccuracy = aoeHit / aoeCount;
    }

    void OnDashLanded(float value)
    {
        dashHit++;
        dashAccuracy = dashHit / dashCount;
    }

    void OnJumpLanded(float value)
    {
        jumpHit++;
        jumpAccuracy = jumpHit / jumpCount;
    }

    void OnRangedLanded(float value)
    {
        rangedHit++;
        rangedAccuracy = rangedHit / rangedCount;
    }

    void OnMeleeTaken(float value)
    {
        meleeDmgTaken += value;
    }

    void OnRangedTaken(float value)
    {
        rangeDmgTaken += value;
    }

    void OnDisable()
    {
        OnBonkHit.onBonkHit -= OnBonkLanded;
        OnAoeHit.onAoeHit -= OnAoeLanded;
        OnDashHit.onDashHit -= OnDashLanded;
        OnJumpHit.onJumpHit -= OnJumpLanded;
        OnAgentBulletHit.onAgentBulletHit -= OnRangedLanded;
        OnSwordHit.onSwordHit -= OnMeleeTaken;
        OnPlayerBulletHit.onPlayerBulletHit -= OnRangedTaken;
    }
}
