using System.Collections.Generic;
using UnityEngine;

public class AchievementUISetup : MonoBehaviour
{
    [SerializeField]
    private Sprite _tutorialImage;
    [SerializeField]
    private Sprite _bestiaryImage;
    [SerializeField]
    private Sprite _historyImage;

    private AchievmentHandler _ah;
    private List<Mission> _missions = new List<Mission>();

    private void CreateTutorialMission()
    {
        Mission ms = new Mission(
            "Tutorial",
            "Finalize os tutoriais",
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
            "Bestiário",
            "Mate seus inimigos",
            "Mate todos os inimigos do jogo",
            _bestiaryImage,
            _bestiaryImage,
            _ah.BestiaryCompleted()
        );


        _missions.Add(ms);
    }

    private void CreateHistoryMission()
    {
        Mission ms = new Mission(
            "Historia",
            "Contacte todos os NPCs",
            "Para entender esse vasto mundo de XXX temos que conversar com todos os personagens perdidos nessa densa caverna",
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
        _missions.Clear();

        CreateTutorialMission();
        CreateBestiaryMission();
        CreateHistoryMission();
    }

    public List<Mission> GetMissionsRegistered() {
        UpdateMissions();

        return _missions;
    }
}
