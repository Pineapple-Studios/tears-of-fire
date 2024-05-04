using UnityEngine;

public class BossFightScenarioHandler : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private Animator _dynamicScenarioAnim;
    [SerializeField]
    private PollingDrops _rockDrops;
    [SerializeField]
    private ParticleSystem _leftRocks;
    [SerializeField]
    private ParticleSystem _rightRocks;
    [SerializeField]
    private TargetHandler _targetHandler;
    [SerializeField]
    private VFXDash _vfxDash;

    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = FindFirstObjectByType<PlayerController>();
    }

    private void Start()
    {
        _dynamicScenarioAnim.gameObject.SetActive(false);
    }

    public void RestartScenario()
    {
        _dynamicScenarioAnim.gameObject.SetActive(false);
        _dynamicScenarioAnim.Rebind();
        _dynamicScenarioAnim.Update(0f);
    }

    public void TriggerDynamicScenario()
    {
        _dynamicScenarioAnim.gameObject.SetActive(true);
        _dynamicScenarioAnim.SetBool("isActive", true);
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

    public void FinishTucanoRexIntroduction()
    {
        _playerController.EnableInput();
    }

    public void StartDashEffect()
    {
        if (_vfxDash != null) _vfxDash.TriggerDash();
    }

    public void StopDashEffect()
    {
        if (_vfxDash != null) _vfxDash.TriggerStopDash();
    }
}
