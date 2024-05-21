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

        // ToDo: 
        // Black screen
        // DisableOnGamePlayInputs
        // Trasição de chamas
        // EnableOnGamePlayInputs
    }

    public void ExitGame()
    {
        Destroy(FeedbackAndDebugManager.Instance.gameObject);
        Destroy(LevelDataManager.Instance.gameObject);
        Destroy(FMODAudioManager.Instance.gameObject);
        Destroy(CinemachineShakeManager.Instance.gameObject);
        Destroy(ScenarioColorManager.Instance.gameObject);
    }
}
