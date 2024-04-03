using System.Collections;
using UnityEngine;

public class MovimentTutorial : MonoBehaviour
{
    [SerializeField]
    private float _timeToTriggerModal = 2f;
    [SerializeField]
    private LayerMask _playerLayer;
    [SerializeField]
    private Animator _modalAnimator;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & _playerLayer) != 0))
        {
            StartCoroutine("ShowModalCoroutine");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

    private IEnumerator ShowModalCoroutine()
    {
        yield return new WaitForSeconds(_timeToTriggerModal);
        _modalAnimator.SetBool("isShowModal", true);
    }
}
