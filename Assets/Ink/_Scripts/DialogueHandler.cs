using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueHandler : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    GameObject player;

    [Header("NPC")]
    [SerializeField]
    GameObject yke;

    [SerializeField]
    GameObject dialogueManagerYKE;

    [SerializeField]
    GameObject oce;
    [SerializeField]
    GameObject dialogueManagerOCE;

    [SerializeField]
    GameObject yxo;
    [SerializeField]
    GameObject dialogueManagerYXO;

    [SerializeField]
    GameObject secondYxo;
    [SerializeField]
    GameObject secondDialogueManagerYXO;

    [Header("TextBox")]
    [SerializeField]
    GameObject textBox;

    [SerializeField]
    GameObject subtitleInteraction;

    [SerializeField]
    TextMeshProUGUI showInteraction;

    string textInteraction;

    [Header("Lore")]
    [SerializeField]
    TextMeshProUGUI textLore;

    [SerializeField]
    GameObject BG;

    [SerializeField]
    float disableTime = 3;
    
    private float _time;
    private bool _disable;

    [Header("InputSystem")]
    private InputActionAsset actions;

    private void Awake()
    {
        //Actions.FindActionMap("").FindAction("").performed += ;
    }

    private void OnEnable()
    {
        //Actions.FindActionMap("").Enable();
    }

    private void OnDisable()
    {
        //Actions.FindActionMap("").Disable();
    }

    private void Start()
    {
        textBox.SetActive(false);
        subtitleInteraction.SetActive(false);
        dialogueManagerYKE.GetComponent<DialogueManager>().enabled = false;
        dialogueManagerOCE.GetComponent<DialogueManager>().enabled = false;
        dialogueManagerYXO.GetComponent<DialogueManager>().enabled = false;
        secondDialogueManagerYXO.GetComponent<DialogueManager>().enabled = false;
        textInteraction = "Press \"E\" to interact with NPC";
        showInteraction.text = textInteraction;
    }
    void Update()
    {
        ShowLore();
        DialogueNPC();
        OutRangeNPC();
    }

    void DialogueNPC()
    {
       if (Mathf.Abs(player.transform.position.x - yke.transform.position.x) <= 5.0f)
       {
            subtitleInteraction.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                textInteraction = "Press \"Enter\" to continue";
                showInteraction.text = textInteraction;
                textBox.SetActive(true);
                dialogueManagerYKE.GetComponent<DialogueManager>().enabled = true;
                Time.timeScale = 0;
                Debug.Log("Dialogo YKE");
            }
        }
       
       if (Mathf.Abs(player.transform.position.x - oce.transform.position.x) <= 5.0f)
       {
            subtitleInteraction.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                textInteraction = "Press \"Enter\" to continue";
                showInteraction.text = textInteraction;
                textBox.SetActive(true);
                dialogueManagerOCE.GetComponent<DialogueManager>().enabled = true;
                Time.timeScale = 0;
                Debug.Log("Dialogo OCE");
            }
        }

        if (Mathf.Abs(player.transform.position.x - yxo.transform.position.x) <= 5.0f)
        {
            subtitleInteraction.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                textInteraction = "Press \"Enter\" to continue";
                showInteraction.text = textInteraction;
                textBox.SetActive(true);
                dialogueManagerYXO.GetComponent<DialogueManager>().enabled = true;
                Time.timeScale = 0;
                Debug.Log("Dialogo YXO");
            }
        }

        if (Mathf.Abs(player.transform.position.x - secondYxo.transform.position.x) <= 5.0f)
        {
            subtitleInteraction.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                textInteraction = "Press \"Enter\" to continue";
                showInteraction.text = textInteraction;
                textBox.SetActive(true);
                secondDialogueManagerYXO.GetComponent<DialogueManager>().enabled = true;
                Time.timeScale = 0;
                Debug.Log("Segundo Dialogo YXO");
            }
        }
    }

    void OutRangeNPC()
    {
        if (Mathf.Abs(player.transform.position.x - yke.transform.position.x) >= 5.0f)
        {
            textInteraction = "Press \"E\" to interact with NPC";
            showInteraction.text = textInteraction;
            dialogueManagerYKE.GetComponent<DialogueManager>().enabled = false;
        }

        if (Mathf.Abs(player.transform.position.x - oce.transform.position.x) >= 5.0f)
        {
            textInteraction = "Press \"E\" to interact with NPC";
            showInteraction.text = textInteraction;
            dialogueManagerOCE.GetComponent<DialogueManager>().enabled = false;
        }

        if (Mathf.Abs(player.transform.position.x - yxo.transform.position.x) >= 5.0f)
        {
            textInteraction = "Press \"E\" to interact with NPC";
            showInteraction.text = textInteraction;
            dialogueManagerYXO.GetComponent<DialogueManager>().enabled = false;
        }

        if (Mathf.Abs(player.transform.position.x - secondYxo.transform.position.x) >= 5.0f)
        {
            textInteraction = "Press \"E\" to interact with NPC";
            showInteraction.text = textInteraction;
            secondDialogueManagerYXO.GetComponent<DialogueManager>().enabled = false;
        }
        subtitleInteraction.SetActive(false);

    }

    void ShowLore()
    {
        if (_disable)
        {
            _time = 0;
        }
        if (!_disable)
        {
            _time += Time.deltaTime;
        }
        if (_time >= disableTime)
        {
            textLore.enabled = false;
            BG.SetActive(false);
            _time = 0;
            _disable = true;
        }
    }
}
