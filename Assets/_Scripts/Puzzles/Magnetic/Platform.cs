using FMOD.Studio;
using FMODUnity;
using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]
    private LayerMask _playerLayer;

    public bool anim_isAnimating = false;
        
    private PlayerController _pc;
    private PlayerPuzzleHandler _pph;

    private Vector3 _initialPos = Vector3.zero;
    private bool isApplingForce = false;
    private bool isApplicable = false;

    private void OnBecameVisible()
    {
        isApplicable = true;
    }

    private void OnBecameInvisible()
    {
        isApplicable = false;
    }

    private void OnEnable()
    {
        LevelDataManager.onRestartElements += OnRestart;
    }

    private void OnDisable()
    {
        LevelDataManager.onRestartElements -= OnRestart;
    }

    private void OnRestart()
    {
        _pc.IncreaseExternalVelocity(Vector2.zero);
        isApplicable = false;
    }

    void Start()
    {
        _pc = FindAnyObjectByType<PlayerController>();
        _pph = FindAnyObjectByType<PlayerPuzzleHandler>();
        _initialPos = transform.position;
    }

    private void Update()
    {
        // Start
        if (isApplicable && isApplingForce && _pph.IsInPlatform()) UpdateVelocity();
        // Stop
        if (isApplingForce && !_pph.IsInPlatform()) isApplingForce = false; 
    }

    private void UpdateVelocity()
    {
        // Calc velocity based on Position
        Vector2 vel = (transform.position - _initialPos) / Time.deltaTime;
        _initialPos = transform.position;

        if (!anim_isAnimating && vel != Vector2.zero) vel = Vector2.zero;

        // Y force is applied based on collisions between platform and player
        _pc.IncreaseExternalVelocity(new Vector2(vel.x, 0));
    }

    EventInstance sound;
    /// <summary>
    /// Método chamado ao acabar o movimento da animação
    /// </summary>
    public void EndPlatformMoviment()
    {
        _pc.IncreaseExternalVelocity(Vector2.zero);
    }

    public void StartPlatformMoviment()
    {

    }

    public void ReleaseSound()
    {
        sound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        sound.release();
    }

    /// <summary>
    /// If player is in platform add force to player
    /// </summary>
    public void OnStartMovementByTrigger()
    {
        isApplingForce = true;
    }
}
