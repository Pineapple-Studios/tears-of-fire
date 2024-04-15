using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public event Action KeyJumpDown;
    public event Action KeyJumpUp;
    public event Action KeyAttackDown;
    public event Action KeyAttackUp;
    public event Action KeyDashDown;
    public event Action KeyDashUp;
    public event Action KeyNPCInteractionDown;
    public event Action KeyNPCInteractionUp;

    private const string GAMEPLAY_ACTIONS = "Gameplay";
    // ActionNames
    private const string MOVE_NAME = "Movement";
    private const string ATTACK_NAME = "Attack";
    private const string POWERUP_NAME = "PowerUp";
    private const string INVENTORY_NAME = "Inventory";
    private const string NPC_INTERACTION_NAME = "NPCInteraction";
    private const string JUMP_NAME = "Jump";

    [Header("Actions")]
    [SerializeField]
    private InputActionAsset _actionsAsset;

    // Storing input values
    private InputAction _movement;
    private InputAction _attack;
    private InputAction _jump;
    private InputAction _powerUp;
    private InputAction _npcInteraction;
    // TODO
    private InputAction _inventory;
    

    private PlayerController _pc;
    private InputActionMap _gamePlayMap;

    private void Awake()
    {
        _pc = FindAnyObjectByType<PlayerController>();
        _gamePlayMap = _actionsAsset.FindActionMap(GAMEPLAY_ACTIONS);
        if (_gamePlayMap == null) return;

        _movement = _gamePlayMap.FindAction(MOVE_NAME);
        _inventory = _gamePlayMap.FindAction(INVENTORY_NAME);

        _jump = _gamePlayMap.FindAction(JUMP_NAME);
        _jump.performed += OnKeyJumpDown;
        _jump.canceled += OnKeyJumpUp;

        _attack = _gamePlayMap.FindAction(ATTACK_NAME);
        _attack.performed += OnKeyAttackDown;
        _attack.canceled += OnKeyAttackUp;

        // Dash
        _powerUp = _gamePlayMap.FindAction(POWERUP_NAME);
        _powerUp.performed += OnKeyDashDown;
        _powerUp.canceled += OnKeyDashUp;

        _npcInteraction = _gamePlayMap.FindAction(NPC_INTERACTION_NAME);
        _npcInteraction.performed += OnKeyNPCInteractionDown;
        _npcInteraction.canceled += OnKeyNPCInteractionUp;
    }

    protected virtual void OnKeyJumpDown(InputAction.CallbackContext context) {
        if (_pc != null && _pc.ShouldDisbleInput()) return;
        KeyJumpDown?.Invoke(); 
    }
    protected virtual void OnKeyJumpUp(InputAction.CallbackContext context) {
        if (_pc != null && _pc.ShouldDisbleInput()) return;
        KeyJumpUp?.Invoke(); 
    }
    
    protected virtual void OnKeyAttackDown(InputAction.CallbackContext context) {
        if (_pc != null && _pc.ShouldDisbleInput()) return;
        KeyAttackDown?.Invoke(); 
    }

    protected virtual void OnKeyAttackUp(InputAction.CallbackContext context) {
        if (_pc != null && _pc.ShouldDisbleInput()) return; 
        KeyAttackUp?.Invoke(); 
    }

    protected virtual void OnKeyDashDown(InputAction.CallbackContext context) {
        if (_pc != null && _pc.ShouldDisbleInput()) return; 
        KeyDashDown?.Invoke(); 
    }

    protected virtual void OnKeyDashUp(InputAction.CallbackContext context) {
        if (_pc != null && _pc.ShouldDisbleInput()) return; 
        KeyDashUp?.Invoke(); 
    }

    protected virtual void OnKeyNPCInteractionDown(InputAction.CallbackContext context) { KeyNPCInteractionDown?.Invoke(); }
    protected virtual void OnKeyNPCInteractionUp(InputAction.CallbackContext context) { KeyNPCInteractionUp?.Invoke(); }
    public Vector2 GetDirection()
    {
        if (_pc != null && _pc.ShouldDisbleInput()) return Vector2.zero;

        return _movement.ReadValue<Vector2>();
    }

    public void DisableInputs()
    {
        if (_pc == null) return;
        _pc.DisableInput();
    }
    

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
