using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public static Action onJump, onDash;
    public static Action onAttack, onSwap;
    public static Action<float> onShield;
    public static Action<Vector2> onMove;
    KeyboardAndMouse actions;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            actions = new();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        actions.KBM.Enable();
        actions.KBM.Move.performed += OnMove;
        actions.KBM.Move.canceled += OnMove;
        actions.KBM.Jump.performed += OnJump;
        actions.KBM.Dash.performed += OnDash;
        actions.KBM.Attack.performed += OnAttack;
        actions.KBM.Swap.performed += OnSwap;
        actions.KBM.Shield.performed += OnShield;
        actions.KBM.Shield.canceled += OnShield;
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        onMove?.Invoke(ctx.ReadValue<Vector2>());
    }

    void OnJump(InputAction.CallbackContext ctx)
    {
        onJump?.Invoke();
    }

    void OnDash(InputAction.CallbackContext ctx)
    {
        onDash?.Invoke();
    }

    void OnAttack(InputAction.CallbackContext ctx)
    {
        onAttack?.Invoke();
    }

    void OnSwap(InputAction.CallbackContext ctx)
    {
        onSwap?.Invoke();
    }

    void OnShield(InputAction.CallbackContext ctx)
    {
        onShield?.Invoke(ctx.ReadValue<float>());
    }

    void OnDisable()
    {
        actions.KBM.Disable();
        actions.KBM.Move.performed -= OnMove;
        actions.KBM.Move.canceled -= OnMove;
        actions.KBM.Jump.performed -= OnJump;
        actions.KBM.Dash.performed -= OnDash;
        actions.KBM.Attack.performed -= OnAttack;
        actions.KBM.Swap.performed -= OnSwap;
        actions.KBM.Shield.performed -= OnShield; 
        actions.KBM.Shield.canceled -= OnShield;
    }
}
