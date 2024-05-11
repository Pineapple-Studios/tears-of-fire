using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWalkStart : MonoBehaviour, IWalkStart
{
    private const string DEATH_EVENT = "event:/Tutorial/Enemy/Spider/SFX_Death";

    public void OnStartWalking() { }

    public void ResetWalk() { }

    public void PlayDeathSound()
    {
        if (FMODAudioManager.Instance != null)
            FMODAudioManager.Instance.PlayOneShot(DEATH_EVENT, this.transform.position);
    }

    public void PlayAttackSound() { }
}
