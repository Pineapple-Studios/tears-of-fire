using System.Collections;
using UnityEngine;

public class DoorLockedFeedbackHandler : MonoBehaviour
{
    [Header("Feedback")]
    [SerializeField]
    private GameObject _cvFeedback;

    [Header("Game elements")]
    [SerializeField]
    private GameObject _door;
    [SerializeField]
    private TucanoRex _kwy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        if (go.tag == "Player")
        {
            if (!HasKey(go)) _cvFeedback.SetActive(true);
            else
            {
                _cvFeedback.SetActive(false);
                _door.SetActive(false); // Devemos substituir isso aqui por uma animação
                _kwy.StartTucanoRex();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        if (go.tag == "Player")
        {
            if (!HasKey(go)) _cvFeedback.SetActive(false);
        }
    }

    private bool HasKey(GameObject obj)
    {
        PlayerItems pi = obj.GetComponentInChildren<PlayerItems>();
        if (pi == null) return false;

        return pi.HasKwyRoomKey();
    }
}
