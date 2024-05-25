using UnityEngine;
using UnityEngine.SceneManagement;

public class StartEndGameController : MonoBehaviour
{
    public static StartEndGameController Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void StartCaveScenario()
    {
        if (FeedbackAndDebugManager.Instance != null)
            FeedbackAndDebugManager.Instance.StartGame();

        if (LevelDataManager.Instance != null)
            LevelDataManager.Instance.InitStates();

        if (FMODAudioManager.Instance != null)
            FMODAudioManager.Instance.SetInitialValues();

        if (CinemachineShakeManager.Instance != null)
            CinemachineShakeManager.Instance.StartManager();
    }

    public void ExitGame()
    {
        if (FeedbackAndDebugManager.Instance != null)
            Destroy(FeedbackAndDebugManager.Instance.gameObject);

        if (LevelDataManager.Instance != null)
            Destroy(LevelDataManager.Instance.gameObject);

        if (FMODAudioManager.Instance != null)
            Destroy(FMODAudioManager.Instance.gameObject);

        if (CinemachineShakeManager.Instance != null)
            Destroy(CinemachineShakeManager.Instance.gameObject);

        if (ScenarioColorManager.Instance != null)
            Destroy(ScenarioColorManager.Instance.gameObject);
    }
}
