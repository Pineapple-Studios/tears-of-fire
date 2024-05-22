using Ink.Runtime;
using System;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class NPC : MonoBehaviour
{
    private const string LOC_ENGLISH_ID = "English (en)";

    public static Action<NPC> StartDialogOf;
    public static Action<NPC> FinishNPCDialog;

    [Header("Alerts")]
    [SerializeField]
    private bool _shouldNotifyOnFinish = false;

    [Header("Properties")]
    [SerializeField]
    public string NpcName;
    [SerializeField]
    public Vector3 CameraOffset = Vector3.zero;
    [Header("Art")]
    [SerializeField]
    public Sprite DialogConcept;
    [SerializeField]
    public Vector3 ConceptPosition = Vector3.zero;
    [Header("Localization")]
    [SerializeField]
    private TextAsset _inkFilePT;
    [SerializeField]
    private TextAsset _inkFileEN;

    [Header("References")]
    [SerializeField]
    private LayerMask _playerLayer;
    [SerializeField]
    private GameObject _floatingButton;

    public Story DialogStory;

    private bool _firstInteraction =  false;
    private bool _isOnRightInteractbleArea = false;
    private PlayerInputHandler _pih;
    private bool _isConversationStarted = false;
    // Please take care whenever you want to use this
    private bool _forceDisabledNPC = false;

    private void Awake()
    {
        _pih = FindAnyObjectByType<PlayerInputHandler>();
    }

    private void OnEnable()
    {
        if (_pih != null) _pih.KeyNPCInteractionDown += OnNPCInteraction;
    }

    private void OnDisable()
    {
        if (_pih != null) _pih.KeyNPCInteractionDown -= OnNPCInteraction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _playerLayer) != 0 && !_firstInteraction)
        {
            _isOnRightInteractbleArea = true;
            OnNPCInteraction();
            _firstInteraction = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_forceDisabledNPC) return;

        if (((1 << collision.gameObject.layer) & _playerLayer) != 0 && !_isConversationStarted)
        {
            _isOnRightInteractbleArea = true;
            if (!_floatingButton.activeSelf) _floatingButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _playerLayer) != 0)
        {
            _isOnRightInteractbleArea = false;
            if (_floatingButton.activeSelf) _floatingButton.SetActive(false);
        }
    }

    private void Update()
    {
        if (_forceDisabledNPC && _floatingButton.activeSelf) _floatingButton.SetActive(false);
    }

    private void OnNPCInteraction()
    {
        if (_forceDisabledNPC) return;
        if (!_isOnRightInteractbleArea || _isConversationStarted) return;

        DialogStory = GetStoryByLocale();
        StartDialogOf(this);
        _isConversationStarted = true;

        _floatingButton.SetActive(false);
    }

    private Story GetStoryByLocale()
    {
        string loc = LocalizationSettings.SelectedLocale.name;

        if (loc == LOC_ENGLISH_ID && _inkFileEN != null)
        {
             return new Story(_inkFileEN.text);
        }
        else
        {
            return new Story(_inkFilePT.text);
        }
    }

    public void FinishDialog()
    {
        _isConversationStarted = false;
        if (_shouldNotifyOnFinish) FinishNPCDialog(this);
    }

    public bool IsConversationStarted() => _isConversationStarted;

    public void ForceDisabledNPC()
    {
        _forceDisabledNPC = true;
    }
}
