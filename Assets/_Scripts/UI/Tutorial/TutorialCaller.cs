using System;
using UnityEngine;

public enum ETutorialAvailable
{
    ATTACK,
    JUMP,
    MOVE,
    TURN_ON
}

public class TutorialCaller : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private TutorialController _tc;

    [Header("Props")]
    [SerializeField]
    private ETutorialAvailable _tutorial;

    void Start()
    {
        _tc.Clean();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") CallTutorial();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") _tc.HiddenTutorial(_tutorial);
    }

    private void CallTutorial()
    {
        switch(_tutorial)
        {
            case ETutorialAvailable.ATTACK:
                _tc.AttackTutorial();
                break;
            case ETutorialAvailable.JUMP:
                _tc.JumpTutorial();
                break;
            case ETutorialAvailable.MOVE:
                _tc.MoveTutorial();
                break;
            case ETutorialAvailable.TURN_ON:
                _tc.TurnOnTutorial();
                break;
            default:
                return;
        }
        
    }
}
