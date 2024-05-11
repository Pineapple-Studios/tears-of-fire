using UnityEngine;

public class NPCAnimationController : MonoBehaviour
{
    private const string TALK_ANIM = "clip_talk";
    private const string IDLE_ANIM = "clip_idle";

    private Animator _anim;
    private NPC _npc;

    private bool _isTalking = false;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _npc = GetComponent<NPC>(); 
    }

    private void Update()
    {
        if (_npc.IsConversationStarted() && !_isTalking) StartTalk();
        if (_isTalking && !_npc.IsConversationStarted()) StopTalk();
    }

    private void StartTalk()
    {
        _isTalking = true;
        _anim.Play(TALK_ANIM);
    }

    private void StopTalk()
    {
        _isTalking = false;
        _anim.Play(IDLE_ANIM);
    }

}
