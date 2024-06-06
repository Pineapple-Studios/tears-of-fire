using FMOD.Studio;
using UnityEngine;

public class SoundHandlerPlatform : MonoBehaviour
{
    bool isApplicable;
    EventInstance sound;
    
    private void OnBecameVisible()
    {
        isApplicable = true;
    }

    private void OnBecameInvisible()
    {
        isApplicable = false;
    }


    public void EndPlatformMove()
    {
        if (isApplicable)
        {
            sound.setParameterByName("Stop", 1.0f);
            Debug.Log("Stop");
        }
    }

    public void StartPlatformMove()
    {
        if (isApplicable)
        {
            sound = FMODAudioManager.Instance.PlayOneShotWithParameters("event:/Tutorial/Mecanicas/SFX_Magnetism", transform.position, ("Stop", 0.0f));
            Debug.Log("MOVE");
        }
    }
}
