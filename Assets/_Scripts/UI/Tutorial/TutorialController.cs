using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [Header("Tutorial reference")]
    [SerializeField]
    private GameObject _animatedElement;
    [SerializeField]
    private Animator _anim;

    [Header("Setup")]
    [SerializeField]
    private Vector3 _offset = Vector3.zero;

    [Header("Tutorial keys")]
    [SerializeField]
    private GameObject _tutorialAttack;
    [SerializeField]
    private GameObject _tutorialJump;
    [SerializeField]
    private GameObject _tutorialMovement;
    [SerializeField]
    private GameObject _tutorialTurnOn;

    private bool _isAttackFinished = false;
    private bool _isJumpFinished = false;
    private bool _isMovementFinished = false;
    private bool _isTurnOnFinished = false;

    private PlayerInputHandler _pih;
    private AchievmentHandler _ah;

    private void Awake()
    {
        _pih = FindAnyObjectByType<PlayerInputHandler>();
        _ah = FindAnyObjectByType<AchievmentHandler>();
    }

    private void OnEnable()
    {
        if (_pih != null)
        {
            _pih.KeyJumpDown += AlreadyJump;
            _pih.KeyAttackDown += AlreadyAttack;
        }
    }

    private void OnDisable()
    {
        if (_pih != null)
        {
            _pih.KeyJumpDown -= AlreadyJump;
            _pih.KeyAttackDown -= AlreadyAttack;
        }
    }

    private void Update()
    {
        if (_pih.GetDirection().x != 0 && !_isMovementFinished)
        {
            _isMovementFinished = true;
            _anim.Play("clip_hidden_tutorial");
            _ah.SetCompleteState(_ah.TUTORIAL_MOVE);
        }
    }

    private void AlreadyJump()
    {
        if (_isJumpFinished)
        {
            _ah.SetCompleteState(_ah.TUTORIAL_JUMP);
            return;
        }

        if (!_isJumpFinished) _isJumpFinished = true;
        _anim.Play("clip_hidden_tutorial");
        _ah.SetCompleteState(_ah.TUTORIAL_JUMP);
    }

    private void AlreadyAttack()
    {
        if (_isAttackFinished)
        {
            _ah.SetCompleteState(_ah.TUTORIAL_ATTACK);
            return;
        }

        if (!_isAttackFinished) _isAttackFinished = true;
        _anim.Play("clip_hidden_tutorial");
        _ah.SetCompleteState(_ah.TUTORIAL_ATTACK);
    }

    private void PlayTutorial(GameObject tutorialElement)
    {
        GameObject obj = Instantiate(tutorialElement, _animatedElement.transform);
        obj.transform.position += _offset;
        obj.transform.parent = _animatedElement.transform;
        _anim.Play("clip_show_tutorial");
    }

    public void AttackTutorial() { if (!_isAttackFinished) PlayTutorial(_tutorialAttack); }
    public void JumpTutorial() { if (!_isJumpFinished) PlayTutorial(_tutorialJump); }
    public void MoveTutorial() { if (!_isMovementFinished) PlayTutorial(_tutorialMovement); }
    public void TurnOnTutorial() { if (!_isTurnOnFinished) PlayTutorial(_tutorialTurnOn); }

    public void HiddenTutorial(ETutorialAvailable kindOf)
    {
        if (
            (kindOf == ETutorialAvailable.ATTACK && _isAttackFinished) ||
            (kindOf == ETutorialAvailable.MOVE && _isMovementFinished) ||
            (kindOf == ETutorialAvailable.JUMP && _isJumpFinished) ||
            (kindOf == ETutorialAvailable.TURN_ON && _isTurnOnFinished)
        ) return;

        _anim.Play("clip_hidden_tutorial");
    }

    public void Clean()
    {
        if (_animatedElement.transform.childCount > 0)
            Destroy(_animatedElement.transform.GetChild(0).gameObject);
    }

}
