using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerHealth : MonoBehaviour
{
    public static Action gameEnd;
    [SerializeField] Slider healthSlider;
    [SerializeField] float maxPlayerHealth;
    [SerializeField] GameObject gameOverText;

    float playerHealth;

    void Start()
    {
        playerHealth = maxPlayerHealth;
        healthSlider.maxValue = maxPlayerHealth;
        healthSlider.value = playerHealth;
    }

    void OnEnable()
    {
        OnBonkHit.onBonkHit += OnDecrementHealth;
        OnAoeHit.onAoeHit += OnDecrementHealth;
        OnDashHit.onDashHit += OnDecrementHealth;
        OnJumpHit.onJumpHit += OnDecrementHealth;
        OnAgentBulletHit.onAgentBulletHit += OnDecrementHealth;
    }

    void OnDecrementHealth(float value)
    {
        playerHealth -= value;
        healthSlider.value = playerHealth;
        if (playerHealth <= 0)
        {
            gameObject.SetActive(false);
            gameEnd?.Invoke();
            gameOverText.SetActive(true);
        }
    }

    void OnDisable()
    {
        OnBonkHit.onBonkHit -= OnDecrementHealth;
        OnAoeHit.onAoeHit -= OnDecrementHealth;
        OnDashHit.onDashHit -= OnDecrementHealth;
        OnJumpHit.onJumpHit -= OnDecrementHealth;
        OnAgentBulletHit.onAgentBulletHit -= OnDecrementHealth;
    }
}
