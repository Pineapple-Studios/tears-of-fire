using System.Collections;
using UnityEngine;

public class WormWalkStart : MonoBehaviour, IWalkStart
{
    private const string DEATH_EVENT = "event:/Tutorial/Enemy/Crawler/SFX_Death";
    private const string ATTACK_EVENT = "event:/Tutorial/Enemy/Crawler/SFX_Atk";

    [SerializeField]
    private float _delayToStartWalking = 0.5f;

    private bool _isStarted = false;

    private AchievmentHandler _ah;

    private void Awake()
    {
        _ah = FindAnyObjectByType<AchievmentHandler>();
    }

    public void OnDead()
    {
        _ah.SetCompleteState(_ah.WORM);
    }


    public void OnStartWalking()
    {
        if (!_isStarted) StartCoroutine(DelayToStart());
    }

    private IEnumerator DelayToStart()
    {
        yield return new WaitForSeconds(_delayToStartWalking);
        GetComponent<Enemy>().StartWalk();
        _isStarted = true;
    }

    public void ResetWalk()
    {
        _isStarted = false;
    }

    public void PlayDeathSound()
    {
        if (FMODAudioManager.Instance != null)
            FMODAudioManager.Instance.PlayOneShot(DEATH_EVENT, this.transform.position);
    }

    public void PlayAttackSound()
    {
        if (FMODAudioManager.Instance != null)
            FMODAudioManager.Instance.PlayOneShot(ATTACK_EVENT, this.transform.position);
    }

    
}