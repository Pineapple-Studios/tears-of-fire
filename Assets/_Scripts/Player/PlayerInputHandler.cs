using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public event Action KeyJumpDown;
    public event Action KeyJumpUp;
    public event Action KeyAttackDown;
    public event Action KeyAttackUp;

    private const string GAMEPLAY_ACTIONS = "Gameplay";
    // ActionNames
    private const string MOVE_NAME = "Movement";
    private const string ATTACK_NAME = "Attack";
    private const string POWERUP_NAME = "PowerUp";
    private const string INVENTORY_NAME = "Inventory";
    private const string MAP_NAME = "Map";
    private const string JUMP_NAME = "Jump";

    [Header("Actions")]
    [SerializeField]
    private InputActionAsset _actionsAsset;

    // Storing input values
    private InputAction _movement;
    private InputAction _attack;
    private InputAction _jump;
    // TODO
    private InputAction _inventory;
    private InputAction _powerUp;
    private InputAction _map;
    

    private InputActionMap _gamePlayMap;

    private void Awake()
    {
        _gamePlayMap = _actionsAsset.FindActionMap(GAMEPLAY_ACTIONS);
        if (_gamePlayMap == null) return;

        _movement = _gamePlayMap.FindAction(MOVE_NAME);
        _inventory = _gamePlayMap.FindAction(INVENTORY_NAME);
        _powerUp = _gamePlayMap.FindAction(POWERUP_NAME);
        _map = _gamePlayMap.FindAction(MAP_NAME);

        _jump = _gamePlayMap.FindAction(JUMP_NAME);
        _jump.performed += OnKeyJumpDown;
        _jump.canceled += OnKeyJumpUp;

        _attack = _gamePlayMap.FindAction(ATTACK_NAME);
        _attack.performed += OnKeyAttackDown;
        _attack.canceled += OnKeyAttackUp;
    }

    protected virtual void OnKeyJumpDown(InputAction.CallbackContext context) { KeyJumpDown?.Invoke(); }
    protected virtual void OnKeyJumpUp(InputAction.CallbackContext context) { KeyJumpUp?.Invoke(); }
    protected virtual void OnKeyAttackDown(InputAction.CallbackContext context) { KeyAttackDown?.Invoke(); }
    protected virtual void OnKeyAttackUp(InputAction.CallbackContext context) { KeyAttackUp?.Invoke(); }
    public Vector2 GetDirection() => _movement.ReadValue<Vector2>();

    private void OnEnable()
    {
        if (_gamePlayMap == null) return;

        _gamePlayMap.Enable();
    }

    private void OnDisable()
    {
        if (_gamePlayMap == null) return;

        _gamePlayMap.Disable();
    }
}
