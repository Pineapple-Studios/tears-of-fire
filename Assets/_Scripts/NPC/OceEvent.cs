using Unity.VisualScripting;
using UnityEngine;

public class OceEvent : MonoBehaviour
{
    [SerializeField]
    private GameObject _breakableBlock;

    private Animator _anim;
    private bool _isAlreadyCrying = false;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!_isAlreadyCrying && _breakableBlock.IsDestroyed())
        {
            _isAlreadyCrying = true;
            _anim.Play("clip_cry");
        }

        if (_isAlreadyCrying)
        {
            AnimatorClipInfo[] currentClips = _anim.GetCurrentAnimatorClipInfo(0);

            foreach (AnimatorClipInfo clipInfo in currentClips)
            {
                if (clipInfo.clip.name == "clip_idle") { 
                    _isAlreadyCrying = false; 
                }
            }
        }
    }
}
