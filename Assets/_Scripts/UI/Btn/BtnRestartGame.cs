using UnityEngine;

public class BtnRestartGame : MonoBehaviour
{
    private void OnStart()
    {
        if (FeedbackAndDebugManager.Instance != null)
            FeedbackAndDebugManager.Instance.StartGame();

        if (LevelDataManager.Instance != null)
            LevelDataManager.Instance.InitStates();
    }
}
