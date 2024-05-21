using UnityEngine;

public class YkeEvent : MonoBehaviour
{
    [SerializeField]
    private GameObject _firstYxo;
    [SerializeField]
    private GameObject _secondYxo;
    [SerializeField]
    private Animator _successFeedbackAnim;

    private Animator _anim;
    private PlayerProps _pp;
    private PlayerInputHandler _pih;
    private NPC _me;

    private bool _isInteractionFinished = false;
    private bool _isIdleAnim = false;

    private void Awake()
    {
        _pp = FindAnyObjectByType<PlayerProps>();
        _pih = FindAnyObjectByType<PlayerInputHandler>();
        _anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        NPC.FinishNPCDialog += OnFinishDialog;
        if (_pih != null) _pih.KeyNPCInteractionDown += FinishInteraction;
    }

    private void OnDisable()
    {
        NPC.FinishNPCDialog -= OnFinishDialog;
        if (_pih != null) _pih.KeyNPCInteractionDown -= FinishInteraction;
    }

    private void Update()
    {
        if (_isIdleAnim == true)
        {
            AnimatorClipInfo[] currentClips = _anim.GetCurrentAnimatorClipInfo(0);

            foreach (AnimatorClipInfo clipInfo in currentClips)
            {
                if (clipInfo.clip.name == "clip_idle") { StartInGameCinematic(); }
            }
        }
    }

    private void OnFinishDialog(NPC npc)
    {
        if (npc.NpcName != "Yke") return;

        _me = npc;
        _isIdleAnim = true;
    }

    private void StartInGameCinematic()
    {
        _isIdleAnim = false;

        _pih.DisableInputsOnDialog();
        _me.ForceDisabledNPC();
        _anim.Play("clip_key_show");
    }

    public void OnEndDeadAnimation()
    {
        _successFeedbackAnim.Play("clip_show_feedback");
    }

    public void OnEndFeedbackAnimation()
    {
        _isInteractionFinished = true;
    }

    public void FinishInteraction()
    {
        if (!_isInteractionFinished) return;

        _successFeedbackAnim.Play("clip_hidden");

        if (!LevelDataManager.Instance.GetKwyRoomKey()) FeedbackAndDebugManager.Instance.ToggleKwyKey();
        if (_pp != null && _pp.GetCurrentMaxLife() < 80f) _pp.AddMaxLife(1);

        _firstYxo.SetActive(false);
        _secondYxo.SetActive(true);

        gameObject.SetActive(false);
        _pih.EnableInputs();
    }

}
