using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [Header("Tutorial reference")]
    [SerializeField]
    private GameObject _animatedElement;
    [SerializeField]
    private Animator _anim;

    [Header("Tutorial keys")]
    [SerializeField]
    private GameObject _tutorialAttack;
    [SerializeField]
    private GameObject _tutorialJump;
    [SerializeField]
    private GameObject _tutorialMovement;
    [SerializeField]
    private GameObject _tutorialTurnOn;

    private void PlayTutorial(GameObject tutorialElement)
    {
        GameObject obj = Instantiate(tutorialElement, _animatedElement.transform);
        obj.transform.parent = _animatedElement.transform;
        _anim.Play("clip_show_tutorial");
    }

    public void AttackTutorial() { PlayTutorial(_tutorialAttack); }
    public void JumpTutorial() { PlayTutorial(_tutorialJump); }
    public void MoveTutorial() { PlayTutorial(_tutorialMovement); }
    public void TurnOnTutorial() { PlayTutorial(_tutorialTurnOn); }

    public void HiddenTutorial()
    {
        _anim.Play("clip_hidden_tutorial");
    }

    public void Clean()
    {
        Destroy(_animatedElement.transform.GetChild(0).gameObject);
    }

}
