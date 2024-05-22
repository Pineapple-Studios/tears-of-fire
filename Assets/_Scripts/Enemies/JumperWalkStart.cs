using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperWalkStart : MonoBehaviour, IWalkStart
{
    private const string DEATH_EVENT = "event:/Tutorial/Enemy/Spider/SFX_Death";

    private AchievmentHandler _ah;

    private void Awake()
    {
        _ah = FindAnyObjectByType<AchievmentHandler>();
    }

    public void OnDead()
    {
        _ah.SetCompleteState(_ah.SPIDER);
    }
    public void OnStartWalking()
    {
        
    }

    public void ResetWalk()
    {

    }

    public void PlayDeathSound()
    {
        if (FMODAudioManager.Instance != null)
            FMODAudioManager.Instance.PlayOneShot(DEATH_EVENT, this.transform.position);
    }

    public void PlayAttackSound() { }
}
