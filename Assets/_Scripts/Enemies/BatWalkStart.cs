using UnityEngine;

public class BatWalkStart : MonoBehaviour, IWalkStart
{
    private const string DEATH_EVENT = "event:/Tutorial/Enemy/Bat/SFX_Death";

    private bool _isStarted = false;

    public void OnStartWalking()
    {
        if (!_isStarted && GetComponentInChildren<TrackedMoviment>().enabled)
        {
            // Debug.Log("OnStartWalking");
            GetComponentInChildren<Animator>().Play("clip_idle_walk_transition");
            _isStarted = true;
        }
    }

    public void ResetWalk()
    {
        GetComponentInChildren<Animator>().Play("clip_idle");
        GetComponentInChildren<ToggleComponentByLayerPresence>().ResetState();
        Bat bat = GetComponentInChildren<Bat>();
        bat.ResetState();
        GameObject go = bat.gameObject;
        transform.localPosition = Vector3.zero;
        GetComponentInChildren<TrackedMoviment>().enabled = false;
        go.GetComponent<Collider2D>().enabled = true;
        _isStarted = false;
    }

    public void PlayDeathSound()
    {
        if (FMODAudioManager.Instance != null)
            FMODAudioManager.Instance.PlayOneShot(DEATH_EVENT, this.transform.position);
    }

    public void PlayAttackSound() { }
}
