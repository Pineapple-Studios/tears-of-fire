using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class AchievementUISetup : MonoBehaviour
{
    private const string LOC_ENGLISH_ID = "English (en)";

    [SerializeField]
    private Sprite _tutorialImage;
    [SerializeField]
    private Sprite _bestiaryImage;
    [SerializeField]
    private Sprite _historyImage;

    private AchievmentHandler _ah;
    private List<Mission> _missions = new List<Mission>();

    private bool _isEnglish = false;

    private void CreateTutorialMission()
    {
        Mission ms = new Mission(
            _isEnglish ? "Tutorial" : "Tutorial",
            _isEnglish ? "Finish every tutorial" : "Finalize os tutoriais",
            _isEnglish ? 
            "Let's discover how this game works. Trigger any engine present here once at least":
            "Entendendo como funciona o jogo. Utilize todas as mecânicas pela primeira vez",
            _tutorialImage,
            _tutorialImage,
            _ah.TutorialCompleted()
        );


        _missions.Add(ms);
    }

    private void CreateBestiaryMission()
    {
        Mission ms = new Mission(
            _isEnglish ? "Bestiary" : "Bestiário",
            _isEnglish ? "Burn your enemies" : "Mate seus inimigos",
            _isEnglish ? "Kill any enemy you'll see in game." : "Mate todos os inimigos do jogo",
            _bestiaryImage,
            _bestiaryImage,
            _ah.BestiaryCompleted()
        );


        _missions.Add(ms);
    }

    private void CreateHistoryMission()
    {
        Mission ms = new Mission(
            _isEnglish ? "History": "Historia",
            _isEnglish ? "Talk with everyone": "Contacte todos os NPCs",
            _isEnglish ?
            "To better understand what happened and how do you came here, try to talk with any body you look in that place.":
            "Para entender esse vasto mundo de XXX temos que conversar com todos os personagens perdidos nessa densa caverna.",
            _historyImage,
            _historyImage,
            _ah.HistoryCompleted()
        );


        _missions.Add(ms);
    }

    private void Awake()
    {
        _ah = FindAnyObjectByType<AchievmentHandler>();
        UpdateMissions();
    }

    public void UpdateMissions()
    {
        SetCurrentLenguage();
        _missions.Clear();

        CreateTutorialMission();
        CreateBestiaryMission();
        CreateHistoryMission();
    }

    public List<Mission> GetMissionsRegistered() {
        UpdateMissions();

        return _missions;
    }

    private void SetCurrentLenguage()
    {
        string loc = LocalizationSettings.SelectedLocale.name;
        _isEnglish = loc == null || loc == LOC_ENGLISH_ID;
    }
}
