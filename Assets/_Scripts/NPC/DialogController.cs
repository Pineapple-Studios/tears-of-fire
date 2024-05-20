using TMPro;
using UnityEngine;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    const string ANIM_START = "onStartDialog";
    const string ANIM_END = "onEndDialog";
    const int IN_FOCUS = 12;
    const int OUT_FOCUS = -1;

    [SerializeField]
    private TMP_Text _lblNpcName;
    [SerializeField]
    private TMP_Text _textArea;
    [SerializeField]
    private Image _conceptField;

    private bool _isDialogStarted = false;
    private Animator _anim;
    private string _npcName;
    private Story _story;

    private PlayerInputHandler _pih;
    private NPC _npc;
    private CinemachineVirtualCamera _cvc;

    private void Awake()
    {
        _pih = FindAnyObjectByType<PlayerInputHandler>();
        _cvc = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        NPC.StartDialogOf += StartDialog;
        if (_pih != null) _pih.KeyNPCInteractionDown += ContinueDialog;
    }

    private void OnDisable()
    {
        NPC.StartDialogOf -= StartDialog;
        if (_pih != null) _pih.KeyNPCInteractionDown -= ContinueDialog;
    }

    private void Start()
    {
        _anim = GetComponentInChildren<Animator>();

        ResetController();
    }

    private void Update()
    {
        if (!_isDialogStarted && _anim.GetCurrentAnimatorStateInfo(0).IsName("clip_dialog_running"))
        {
            StartStory();
        }

        UpdateCamOffSet();
    }

    private void UpdateCamOffSet()
    {
        if (_npc == null) return;
        if (_npc.CameraOffset == Vector3.zero) return;

        CinemachineFramingTransposer cft = _cvc.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (_npc.CameraOffset == cft.m_TrackedObjectOffset) return;

        cft.m_TrackedObjectOffset = _npc.CameraOffset;
    }

    private void ContinueDialog()
    {
        if (!_isDialogStarted) return;

        if (_story != null && _story.canContinue)
        {
            _lblNpcName.text = _npcName;
            AdvanceDialogue();
        }
        else
        {
            ResetController();

            _anim.SetTrigger(ANIM_END);
        }
    }

    private void ResetController()
    {
        _npcName = string.Empty;
        if (_story != null) _story.ResetState();
        _story = null;
        _lblNpcName.text = string.Empty;
        _textArea.text = string.Empty;
    }

    private void SetupVirtualCamera()
    {
        if (_cvc == null) return;

        _cvc.Follow = _npc.gameObject.transform;
        _cvc.Priority = IN_FOCUS;
    }

    /// <summary>
    /// Chamado no behaviour de fim da animação do diálogo
    /// </summary>
    public void FinishDialog()
    {
        // Back to main camera
        _cvc.Priority = OUT_FOCUS;

        _npc.FinishDialog();
        _pih.EnableInputs();
        _isDialogStarted = false;
    }

    private void StartDialog(NPC npc)
    {
        _npc = npc;

        SetupThumbnail();
        SetupVirtualCamera();

        _npcName = npc.NpcName;
        _story = npc.DialogStory;

        _pih.DisableInputsOnDialog();
        _pih.gameObject.transform.parent.GetComponent<PlayerController>().FreezeMovement();

        _anim.SetTrigger(ANIM_START);
    }

    private void SetupThumbnail()
    {
        _conceptField.sprite = _npc.DialogConcept;
        _conceptField.SetNativeSize();
        RectTransform rt = _conceptField.gameObject.GetComponent<RectTransform>();
        rt.anchoredPosition = _npc.ConceptPosition;
    }

    private void StartStory()
    {
        _lblNpcName.text = _npcName;
        AdvanceDialogue();

        _isDialogStarted = true;
    }

    private void AdvanceDialogue()
    {
        string currentSentence = _story.Continue();
        
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        _textArea.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            //FMODAudioManager.Instance.PlayOneShot(FMODEventsTutorial.Instance.typing, this.transform.position);
            _textArea.text += letter;
            yield return null;
        }

        yield return null;
    }
}
