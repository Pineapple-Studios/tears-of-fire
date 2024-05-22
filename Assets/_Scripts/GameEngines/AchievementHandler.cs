using UnityEngine;

public class AchievmentHandler : MonoBehaviour
{
    #region Tutorial
    public string TUTORIAL_COMPLETED = "tutorial_completed";
    public string TUTORIAL_MOVE = "move_tutorial";
    public string TUTORIAL_JUMP = "jump_tutorial";
    public string TUTORIAL_ATTACK = "attack_tutorial";
    public string TUTORIAL_TURN_ON = "turn_on_tutorial";
    #endregion

    #region NPC talking
    public string HISTORY_COMPLETED = "history_completed";
    public string YXO_ONE = "yxo_one_talk";
    public string YKE_TWO = "yxo_two_talk";
    public string YKE = "yke_talk";
    public string OCE = "oce_talk";
    public string KWY = "kwy_talk";
    #endregion

    #region Bestiary
    public string BESTIARY_COMPLETED = "bestiary_completed";
    public string WORM = "worm_talk";
    public string SPIDER = "spider_talk";
    public string BAT = "bat_talk";
    public string TUCANOREX = "tucanorex_talk";
    #endregion

    private AchievementHUDHandler _ahh;

    private void Start()
    {
        _ahh = FindAnyObjectByType<AchievementHUDHandler>();
        CleanAll();
    }

    /// <summary>
    /// Verify if the Tutorial is done
    /// </summary>
    public bool TutorialCompleted()
    {
        bool isComplete =
            CheckCompletition(TUTORIAL_MOVE) &&
            CheckCompletition(TUTORIAL_JUMP) &&
            CheckCompletition(TUTORIAL_ATTACK) &&
            CheckCompletition(TUTORIAL_TURN_ON);

        return isComplete;
    }

    /// <summary>
    /// Verify if the History is done
    /// </summary>
    public bool HistoryCompleted()
    {
        bool isComplete =
            CheckCompletition(YXO_ONE) &&
            CheckCompletition(YKE_TWO) &&
            CheckCompletition(YKE) &&
            CheckCompletition(OCE) &&
            CheckCompletition(KWY);

        return isComplete;
    }

    /// <summary>
    /// Verify if the Brestiary is done
    /// </summary>
    public bool BestiaryCompleted()
    {
        bool isComplete =
            CheckCompletition(WORM) &&
            CheckCompletition(SPIDER) &&
            CheckCompletition(BAT) &&
            CheckCompletition(TUCANOREX);

        return isComplete;
    }

    /// <summary>
    /// Set some state of missions is Done
    /// </summary>
    public void SetCompleteState(string state)
    {
        LocalStorage.SetBool(state, true);

        ValidateAchivements();
    }

    private void ValidateAchivements()
    {
        if (TutorialCompleted() && !CheckCompletition(TUTORIAL_COMPLETED))
        {
            _ahh.ShowToast("Tutorial", "Tutorial completo com sucesso!");
            SetCompleteState(TUTORIAL_COMPLETED);
            return;
        }

        if (HistoryCompleted() && !CheckCompletition(HISTORY_COMPLETED))
        {
            _ahh.ShowToast("History", "Historia completa com sucesso!");
            SetCompleteState(HISTORY_COMPLETED);
            return;
        }

        if (BestiaryCompleted() && !CheckCompletition(BESTIARY_COMPLETED))
        {
            _ahh.ShowToast("Bestiary", "Bestiario completo com sucesso!");
            SetCompleteState(BESTIARY_COMPLETED);
            return;
        }
    }

    private bool CheckCompletition(string state)
    {
        return LocalStorage.GetBool(state);
    }

    private void SetIncomplete(string state)
    {
        LocalStorage.SetBool(state, false);
    }

    private void CleanAll()
    {
        // Tutorial
        SetIncomplete(TUTORIAL_COMPLETED);
        SetIncomplete(TUTORIAL_MOVE);
        SetIncomplete(TUTORIAL_JUMP);
        SetIncomplete(TUTORIAL_ATTACK);
        SetIncomplete(TUTORIAL_TURN_ON);
        // History
        SetIncomplete(HISTORY_COMPLETED);
        SetIncomplete(YXO_ONE);
        SetIncomplete(YKE_TWO);
        SetIncomplete(YKE);
        SetIncomplete(OCE);
        SetIncomplete(KWY);
        // Bestiary
        SetIncomplete(BESTIARY_COMPLETED);
        SetIncomplete(WORM);
        SetIncomplete(SPIDER);
        SetIncomplete(BAT);
        SetIncomplete(TUCANOREX);
    }
}

