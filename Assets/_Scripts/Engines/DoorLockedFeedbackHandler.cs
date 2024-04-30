using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class DoorLockedFeedbackHandler : MonoBehaviour
{
    [Header("Feedback")]
    [SerializeField]
    private GameObject _cvFeedback;
    [SerializeField]
    private GameObject _interactionKey;

    [Header("Game elements")]
    [SerializeField]
    private TucanoRex _kwy;
    [SerializeField]
    private Animator _doorAnim;

    private bool _isInteractable;
    private GameObject _player;
    private PlayerInputHandler _playerInputHandler;

    private void Awake()
    {
        _playerInputHandler = FindAnyObjectByType<PlayerInputHandler>();
    }

    private void OnEnable()
    {
        _playerInputHandler.KeyNPCInteractionDown += TryOpen;
    }

    private void OnDisable()
    {
        _playerInputHandler.KeyNPCInteractionDown -= TryOpen;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        if (go.tag == "Player")
        {
            _player = go;
            _isInteractable = true;
            _interactionKey.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        if (go.tag == "Player")
        {
            _isInteractable = false;
            _interactionKey.SetActive(false);
            TryCloseFeedback(go);
        }
    }

    private void TryOpen()
    {
        if (!_isInteractable) return;

        if (!HasKey(_player)) _cvFeedback.SetActive(true);
        else
        {
            _interactionKey.SetActive(false);
            _cvFeedback.SetActive(false);
            _doorAnim.Play("clip_open");
        }
    }

    private void TryCloseFeedback(GameObject player)
    {
        if (!HasKey(player)) _cvFeedback.SetActive(false);
    }

    private bool HasKey(GameObject obj)
    {
        PlayerItems pi = obj.GetComponentInChildren<PlayerItems>();
        if (pi == null) return false;

        return pi.HasKwyRoomKey();
    }
}
