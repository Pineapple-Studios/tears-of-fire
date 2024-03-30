using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueHandler : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    GameObject player;

    [Header("YKE")]
    [SerializeField]
    public GameObject yke;

    [SerializeField]
    GameObject dialogueManagerYKE;

    [SerializeField]
    GameObject interactionYKE;

    [Header("OCE")]
    [SerializeField]
    GameObject oce;

    [SerializeField]
    GameObject dialogueManagerOCE;

    [SerializeField]
    GameObject interactionOCE;

    [Header("YXO")]
    [SerializeField]
    GameObject yxo;

    [SerializeField]
    GameObject dialogueManagerYXO;
    
    [SerializeField]
    GameObject interactionYXO;

    [Header("2ND YXO")]
    [SerializeField]
    GameObject secondYxo;

    [SerializeField]
    GameObject secondDialogueManagerYXO;

    [SerializeField]
    GameObject interaction2ndYXO;

    [Header("TextBox")]
    [SerializeField]
    GameObject textBox;

    [Header("InputSystem")]
    private InputActionAsset actions;

    private string _currentDialog;
    private PlayerController _pc;
    private PlayerProps _pp;
    private bool _isYkeAlreadyDone = false;

    private void Awake()
    {
        //actions.FindActionMap("Dialogue").FindAction("Interaction").performed += OnInteraction;
        _pc = FindFirstObjectByType<PlayerController>();
        _pp = FindFirstObjectByType<PlayerProps>();
    }

    private void OnEnable()
    {
        //actions.FindActionMap("Dialogue").Enable();
        DialogueManager.FinishDialog += OnFinishDialog;
    }

    private void OnDisable()
    {
        //actions.FindActionMap("Dialogue").Disable();
        DialogueManager.FinishDialog -= OnFinishDialog;
    }

    private void OnFinishDialog()
    {
        if (_currentDialog == "yke")
        {
            if (!LevelDataManager.Instance.GetKwyRoomKey()) FeedbackAndDebugManager.Instance.ToggleKwyKey();
            if (_pp != null && _pp.GetCurrentMaxLife() < 80f) _pp.AddMaxLife(1);
            yke.SetActive(false);
            _isYkeAlreadyDone = true;
            _currentDialog = null;
        }

        if (_pc != null) _pc.EnableInput();
    }

    private void Start()
    {
        // NPC's e Caixa de texto
        Handler(textBox, false);
        Handler(secondYxo, false);
        Handler(yxo, true);
        
        // Dialogos
        Dialogue();

        
    }
    void Update()
    {
        DialogueNPC();
        OutRangeNPC();
    }

    void Dialogue()
    {
        dialogueManagerYKE.GetComponentInChildren<DialogueManager>().enabled = false;
        dialogueManagerOCE.GetComponentInChildren<DialogueManager>().enabled = false;
        dialogueManagerYXO.GetComponentInChildren<DialogueManager>().enabled = false;
        secondDialogueManagerYXO.GetComponentInChildren<DialogueManager>().enabled = false;
    }

    void DialogueNPC()
    {
        //if (Mathf.Abs(player.transform.position.x - yke.transform.position.x) <= 20.0f) { Handler(yke, true); }
        if (
            _isYkeAlreadyDone == false && 
            Mathf.Abs(player.transform.position.x - yke.transform.position.x) <= 5.0f
        )
        {
            Handler(interactionYKE, true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                _currentDialog = "yke";
                Handler(textBox, true);
                dialogueManagerYKE.GetComponentInChildren<DialogueManager>().enabled = true;
                if (_pc != null) _pc.DisableInput();
            }
            if (textBox.activeSelf == true) { Handler(interactionYKE, false); }
        }
       
        if (Mathf.Abs(player.transform.position.x - oce.transform.position.x) <= 5.0f)
        {
            Handler(interactionOCE, true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Handler(textBox, true);
                dialogueManagerOCE.GetComponentInChildren<DialogueManager>().enabled = true;
                if (_pc != null) _pc.DisableInput();
            }
            if (textBox.activeSelf == true) { Handler(interactionOCE, false); }
        }

        if (Mathf.Abs(player.transform.position.x - yxo.transform.position.x) <= 5.0f)
        {
            Handler(interactionYXO, true);
            if (Input.GetKeyDown(KeyCode.E))
            { 
                Handler(textBox, true);
                dialogueManagerYXO.GetComponentInChildren<DialogueManager>().enabled = true;
                if (_pc != null) _pc.DisableInput();
            }
            if (textBox.activeSelf == true) { Handler(interactionYXO, false); }
        }

        if (LevelDataManager.Instance.GetKwyRoomKey())   
        {
            Handler(yxo, false);
            Handler(secondYxo, true);
        }

        if (Mathf.Abs(player.transform.position.x - secondYxo.transform.position.x) <= 5.0f)
        {
            Handler(interaction2ndYXO, true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Handler(textBox, true);
                secondDialogueManagerYXO.GetComponentInChildren<DialogueManager>().enabled = true;
                if (_pc != null) _pc.DisableInput();
            }
            if (textBox.activeSelf == true) { Handler(interaction2ndYXO, false); }
        }
    }

    void OutRangeNPC()
    {
        if (Mathf.Abs(player.transform.position.x - yke.transform.position.x) >= 5.0f)
        {
            Handler(interactionYKE, false);
            dialogueManagerYKE.GetComponentInChildren<DialogueManager>().enabled = false;
        }

        if (Mathf.Abs(player.transform.position.x - oce.transform.position.x) >= 5.0f)
        {
            Handler(interactionOCE, false);
            dialogueManagerOCE.GetComponentInChildren<DialogueManager>().enabled = false;
        }

        if (Mathf.Abs(player.transform.position.x - yxo.transform.position.x) >= 5.0f)
        {
            Handler(interactionYXO, false);
            dialogueManagerYXO.GetComponentInChildren<DialogueManager>().enabled = false;
        }

        if (Mathf.Abs(player.transform.position.x - secondYxo.transform.position.x) >= 5.0f)
        {
            Handler(interaction2ndYXO, false);
            secondDialogueManagerYXO.GetComponentInChildren<DialogueManager>().enabled = false;
        }
    }

    private void Handler(GameObject go, bool isActive)
    {
        go.gameObject.SetActive(isActive);
    }
}
