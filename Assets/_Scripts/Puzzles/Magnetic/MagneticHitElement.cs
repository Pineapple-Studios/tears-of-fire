using UnityEngine;

public class MagneticHitElement : MonoBehaviour
{
    [SerializeField]
    private MagneticHandler _mh;

    private Animator _anim;
    private AchievmentHandler _ah;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _ah = FindAnyObjectByType<AchievmentHandler>();
    }

    public void OnNext()
    {
        //if (FMODAudioManager.Instance != null)
        //    FMODAudioManager.Instance.PlayOneShot(
        //        FMODEventsTutorial.Instance.magnetism, 
        //        this.transform.position
        //    );

        _anim.SetTrigger("hasHitted");
        _ah.SetCompleteState(_ah.TUTORIAL_TURN_ON);
        _mh.OnNextStep();
    }
}
