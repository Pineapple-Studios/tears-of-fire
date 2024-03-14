using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class PopUp_Tutorial : MonoBehaviour
{
    [SerializeField] GameObject popupObject;  

    [SerializeField] GameObject popupJump;
    [SerializeField] GameObject popupAttack;

    [Header("Colliders")]
    [SerializeField] Collider2D jumpCollider;
    [SerializeField] Collider2D atkCollider;

    void Start()
    {
        popupAttack.SetActive(false);
        popupJump.SetActive(false);
    }

    private void Update()
    {
        WasPressed();
    }

    void WasPressed()
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            popupAttack.SetActive(false);
            Collider2D collider = atkCollider.GetComponent<Collider2D>();
            collider.enabled = false;
        } 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            popupJump.SetActive(false);
            Collider2D collider = jumpCollider.GetComponent<Collider2D>();
            collider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider has the tag "Player"
        if (collision.gameObject.tag == "Player")
        {
            // Activate the pop-up object
            popupObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            popupObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
