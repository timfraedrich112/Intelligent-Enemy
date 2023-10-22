using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class AgentHealth : MonoBehaviour
{
    public static Action gameEnd;
    [SerializeField] Slider healthSlider;
    [SerializeField] float maxAgentHealth;
    [SerializeField] GameObject victoryText;

    float agentHealth;

    void Start()
    {
        agentHealth = maxAgentHealth;
        healthSlider.maxValue = maxAgentHealth;
        healthSlider.value = agentHealth;
    }

    void OnEnable()
    {
        OnSwordHit.onSwordHit += OnDecrementHealth;
        OnPlayerBulletHit.onPlayerBulletHit += OnDecrementHealth;
    }

    void OnDecrementHealth(float value)
    {
        agentHealth -= value;
        healthSlider.value = agentHealth;
        if (agentHealth <= 0)
        {
            gameObject.SetActive(false);
            gameEnd?.Invoke();
            victoryText.SetActive(true);
        }
    }

    void OnDisable()
    {
        OnSwordHit.onSwordHit -= OnDecrementHealth;
        OnPlayerBulletHit.onPlayerBulletHit -= OnDecrementHealth;
    }
}
