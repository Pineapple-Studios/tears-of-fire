using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Pipes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueHandler : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    GameObject player;

    [Header("YKE")]
    [SerializeField]
    GameObject yke;

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

    private void Awake()
    {
        //actions.FindActionMap("Dialogue").FindAction("Interaction").performed += OnInteraction;
    }

    private void OnEnable()
    {
        //actions.FindActionMap("Dialogue").Enable();
    }

    private void OnDisable()
    {
        //actions.FindActionMap("Dialogue").Disable();
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
        if (Mathf.Abs(player.transform.position.x - yke.transform.position.x) <= 5.0f)
       {
            Handler(interactionYKE, true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Handler(textBox, true);
                dialogueManagerYKE.GetComponentInChildren<DialogueManager>().enabled = true;
                Time.timeScale = 0;
                Debug.Log("Dialogo YKE");
            }
            if (textBox.activeSelf == true) { Handler(interactionYKE, false); }
        }
        if (Mathf.Abs(player.transform.position.x - yke.transform.position.x) <= 5.0f)
        {
            if (Mathf.Abs(player.transform.position.y - yke.transform.position.y) < 2.0f)
            {
                Handler(yxo, false);
                Handler(secondYxo, true);
            }
        }
       
        if (Mathf.Abs(player.transform.position.x - oce.transform.position.x) <= 5.0f)
        {
            Handler(interactionOCE, true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Handler(textBox, true);
                dialogueManagerOCE.GetComponentInChildren<DialogueManager>().enabled = true;
                Time.timeScale = 0;
                Debug.Log("Dialogo OCE");
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
                Time.timeScale = 0;
                Debug.Log("Dialogo YXO");
            }
            if (textBox.activeSelf == true) { Handler(interactionYXO, false); }
        }

        if (Mathf.Abs(player.transform.position.x - secondYxo.transform.position.x) <= 5.0f)
        {
            Handler(interaction2ndYXO, true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Handler(textBox, true);
                secondDialogueManagerYXO.GetComponentInChildren<DialogueManager>().enabled = true;
                Time.timeScale = 0;
                Debug.Log("Segundo Dialogo YXO");
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
