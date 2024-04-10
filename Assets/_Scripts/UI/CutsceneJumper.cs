using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneJumper : MonoBehaviour
{
    [SerializeField]
    VideoPlayer cutscene;

    void Start()
    {
        cutscene.loopPointReached += CutsceneHasEnded;
    }

    void CutsceneHasEnded(VideoPlayer vp)
    {
        GetComponent<MainMenuAnimationController>().GoToGame();
    }
}
