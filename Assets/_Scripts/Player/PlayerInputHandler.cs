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
    public event Action KeyPauseDown;
    public event Action KeyPauseUp;

    private const string GAMEPLAY_ACTIONS = "Gameplay";
    // ActionNames
    private const string MOVE_NAME = "Movement";
    private const string ATTACK_NAME = "Attack";
    private const string POWERUP_NAME = "PowerUp";
    private const string INVENTORY_NAME = "Inventory";
    private const string NPC_INTERACTION_NAME = "NPCInteraction";
    private const string JUMP_NAME = "Jump";
    private const string PAUSE_NAME = "Pause";

    [Header("Actions")]
    [SerializeField]
    private InputActionAsset _actionsAsset;

    // Storing input values
    private InputAction _movement;
    private InputAction _attack;
    private InputAction _jump;
    private InputAction _powerUp;
    private InputAction _npcInteraction;
    private InputAction _pause;
    // TODO
    private InputAction _inventory;

    private PlayerAnimationController _pac;
    private PlayerController _pc;
    private InputActionMap _gamePlayMap;

    private bool _isDisablesInputDialog = false;
    private bool _isDisablesInputGameplay = false;

    private void Awake()
    {
        _pac = FindAnyObjectByType<PlayerAnimationController>();
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

        _pause = _gamePlayMap.FindAction(PAUSE_NAME);
        _pause.performed += OnKeyPauseDown;
        _pause.canceled += OnKeyPauseUp;
    }

    protected virtual void OnKeyJumpDown(InputAction.CallbackContext context) {
        if (_isDisablesInputDialog || _isDisablesInputGameplay) return;
        if (_pc != null && _pc.ShouldDisbleInput()) return;

        KeyJumpDown?.Invoke(); 
    }
    protected virtual void OnKeyJumpUp(InputAction.CallbackContext context) {
        if (_isDisablesInputDialog || _isDisablesInputGameplay) return;
        if (_pc != null && _pc.ShouldDisbleInput()) return;

        KeyJumpUp?.Invoke(); 
    }
    
    protected virtual void OnKeyAttackDown(InputAction.CallbackContext context) {
        if (_isDisablesInputDialog || _isDisablesInputGameplay) return;
        if (_pc != null && _pc.ShouldDisbleInput()) return;

        KeyAttackDown?.Invoke(); 
    }

    protected virtual void OnKeyAttackUp(InputAction.CallbackContext context) {
        if (_isDisablesInputDialog || _isDisablesInputGameplay) return;
        if (_pc != null && _pc.ShouldDisbleInput()) return;

        KeyAttackUp?.Invoke(); 
    }

    protected virtual void OnKeyDashDown(InputAction.CallbackContext context) {
        if (_isDisablesInputDialog || _isDisablesInputGameplay) return;
        if (_pc != null && _pc.ShouldDisbleInput()) return;

        KeyDashDown?.Invoke(); 
    }

    protected virtual void OnKeyDashUp(InputAction.CallbackContext context) {
        if (_isDisablesInputDialog || _isDisablesInputGameplay) return;
        if (_pc != null && _pc.ShouldDisbleInput()) return;

        KeyDashUp?.Invoke(); 
    }

    protected virtual void OnKeyNPCInteractionDown(InputAction.CallbackContext context) {
        if (_isDisablesInputGameplay) return;

        KeyNPCInteractionDown?.Invoke(); 
    }
    protected virtual void OnKeyNPCInteractionUp(InputAction.CallbackContext context) {
        if (_isDisablesInputGameplay) return;

        KeyNPCInteractionUp?.Invoke(); 
    }

    protected virtual void OnKeyPauseDown(InputAction.CallbackContext context)
    {
        if (_isDisablesInputDialog) return;

        DisableInputsOnGameplay();
        KeyPauseDown?.Invoke();
    }

    protected virtual void OnKeyPauseUp(InputAction.CallbackContext context)
    {
        if (_isDisablesInputDialog) return;

        KeyPauseUp?.Invoke();
    }

    public Vector2 GetDirection()
    {
        if (_isDisablesInputDialog || _isDisablesInputGameplay) return Vector2.zero;
        if (_pc != null && _pc.ShouldDisbleInput()) return Vector2.zero;

        return _movement.ReadValue<Vector2>();
    }

    public void DisableInputs()
    {
        if (_pc == null) return;
        _pc.DisableInput();
        _pac.ClearAllStates();
    }

    public void EnableInputs()
    {
        _isDisablesInputDialog = false;
        _isDisablesInputGameplay = false;

        if (_pc == null) return;
        _pc.EnableInput();
    }

    public void DisableInputsOnDialog()
    {
        _isDisablesInputDialog = true;
        DisableInputs();
    }

    public void DisableInputsOnGameplay()
    {
        _isDisablesInputGameplay = true;
        DisableInputs();
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
