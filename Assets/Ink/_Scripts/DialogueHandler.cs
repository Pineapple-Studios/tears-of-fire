using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueHandler : MonoBehaviour
{
    Collider2D[] colliders;

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

    [Header("TextBox")]
    [SerializeField]
    GameObject textBox;

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
        dialogueManagerYKE.SetActive(false);
        dialogueManagerOCE.SetActive(false);
        dialogueManagerYXO.SetActive(false);
    }
    void Update()
    {
        DialogueNPC();

    }

    void DialogueNPC()
    {
       if (Mathf.Abs(player.transform.position.x - yke.transform.position.x) <= 5.0f)
       {
            if (Input.GetKeyDown(KeyCode.E))
            { 
                textBox.SetActive(true);
                dialogueManagerYKE.SetActive(true);
                Time.timeScale = 0;
                Debug.Log("Dialogo YKE");
            }  
       }
       
       if (Mathf.Abs(player.transform.position.x - oce.transform.position.x) <= 5.0f)
       {
            if (Input.GetKeyDown(KeyCode.E))
            {
                textBox.SetActive(true);
                dialogueManagerOCE.SetActive(true);
                Time.timeScale = 0;
                Debug.Log("Dialogo OCE");
            }
       }

        if (Mathf.Abs(player.transform.position.x - yxo.transform.position.x) <= 5.0f)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                textBox.SetActive(true);
                dialogueManagerYXO.SetActive(true);
                Time.timeScale = 0;
                Debug.Log("Dialogo YXO");
            }
        }
    }
}
