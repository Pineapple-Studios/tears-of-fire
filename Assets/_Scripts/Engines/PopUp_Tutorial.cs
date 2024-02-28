using UnityEngine;

public class PopUp_Tutorial : MonoBehaviour
{
    [SerializeField] GameObject popupObject;
    [SerializeField] GameObject ColliderB;

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
        // Check if the exiting collider has the tag "Player"
        if (collision.gameObject.tag == "Player")
        {
            // Destroy or deactivate the pop-up object when the player exits the trigger
            Destroy(popupObject);
            ColliderB.SetActive(true);
            Destroy(this);
            
        }
    }
}
