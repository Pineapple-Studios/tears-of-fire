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
    [SerializeField]
    private TargetHandler _targetHandler;

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

    public void SetTargetNextPos()
    {
        _targetHandler.NextPostion();
    }

    public void StartTarget()
    {
        _targetHandler.gameObject.SetActive(true);
        _targetHandler.Initiate();
    }

    public void LastTarget()
    {
        _targetHandler.gameObject.SetActive(false);
    }
}
