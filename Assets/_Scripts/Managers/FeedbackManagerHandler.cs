using UnityEngine;

public class FeedbackManagerHandler : MonoBehaviour
{
    private const string NEGATIVE_GAMEPLAY_EVENT = "event:/MainMenu/SFX_Negation";

    public static FeedbackManagerHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void NegativeFeedback()
    {
        if (CinemachineShakeManager.Instance != null)
            CinemachineShakeManager.Instance.NegativeFeedback();

        if (RumbleManager.instance != null)
            RumbleManager.instance.NegativeFeedback();

        if (FMODAudioManager.Instance != null)
            FMODAudioManager.Instance.PlayOneShot(NEGATIVE_GAMEPLAY_EVENT, transform.position);
    }
}
