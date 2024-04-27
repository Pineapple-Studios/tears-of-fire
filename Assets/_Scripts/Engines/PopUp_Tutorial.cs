using UnityEngine;

public class PopUp_Tutorial : MonoBehaviour
{
    [SerializeField]
    private TutorialController _tc;
    [SerializeField] 
    private GameObject _UIFeedback;

    // private Collider2D _collider;

    void Start()
    {
        _tc.Clean();

        // _UIFeedback.SetActive(false);
        // _collider = GetComponent<Collider2D>();
    }

    //private void Update()
    //{
    //    WasPressed();
    //}

    //void WasPressed()
    //{
    //    if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Mouse0))
    //    {
    //        popupAttack.SetActive(false);
    //        Collider2D collider = atkCollider.GetComponent<Collider2D>();
    //        collider.enabled = false;
    //    } 

    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        popupJump.SetActive(false);
    //        Collider2D collider = jumpCollider.GetComponent<Collider2D>();
    //        collider.enabled = false;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") _tc.JumpTutorial();   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") _tc.HiddenTutorial();
    }
}
