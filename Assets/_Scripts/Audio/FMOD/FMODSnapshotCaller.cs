using FMODUnity;
using FMOD.Studio;
using UnityEngine;

public class FMODSnapshotCaller : MonoBehaviour
{
    private bool isPlaying = true;

    EventInstance snapshot;

    public void TogglePlayPause()
    {
        if (isPlaying)
        {
            snapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            snapshot.release();
            snapshot = RuntimeManager.CreateInstance("snapshot:/Pause");
            snapshot.start();
            isPlaying = false;
        }
        else
        {
            snapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            snapshot.release();
            snapshot = RuntimeManager.CreateInstance("snapshot:/Play");
            snapshot.start();
            isPlaying = true;
        }
    }
}
