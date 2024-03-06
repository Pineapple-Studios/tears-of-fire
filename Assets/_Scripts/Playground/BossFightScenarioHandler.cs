using UnityEngine;

public class BossFightScenarioHandler : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private PollingDrops _rockDrops;
    [SerializeField]
    private ParticleSystem _leftRocks;
    [SerializeField]
    private ParticleSystem _rightRocks;

    public void RestartScenario()
    {
        _animator.Rebind();
        _animator.Update(0f);
    }

    public void TriggerDynamicScenario()
    {
        _animator.SetBool("isActive", true);
    }

    public void TriggerrRightRocks()
    {
        _rightRocks.Stop();
        _rightRocks.Play();
    }

    public void TriggerrLeftRocks()
    {
        _leftRocks.Stop();
        _leftRocks.Play();
    }

    public void DropRock()
    {
        _rockDrops.StartDroping();
    }
}
