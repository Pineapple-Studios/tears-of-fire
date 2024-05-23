using System.Collections.Generic;
using UnityEngine;

public class InGameMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject _missionCanvas;
    [SerializeField]
    private GameObject _missionButton;
    [SerializeField]
    private GameObject _completedButton;
    [SerializeField]
    private GameObject _buttonGroup;
    [SerializeField]
    private MissionController _mc;

    private bool _isDialogRunning = false;

    private PlayerInputHandler _pih;
    private AchievementUISetup _aus;

    private void Awake()
    {
        _pih = FindAnyObjectByType<PlayerInputHandler>();
        _aus = FindAnyObjectByType<AchievementUISetup>();
    }

    private void OnEnable()
    {
        if (_pih != null) _pih.KeyMissionDown += ToggleMissionMenu;
    }

    private void OnDisable()
    {
        if (_pih != null) _pih.KeyMissionDown -= ToggleMissionMenu;
    }

    /// <summary>
    /// Open and close the Mission Panel
    /// </summary>
    private void ToggleMissionMenu()
    {
        if (_isDialogRunning) return;

        // Vai abrir
        if (!_missionCanvas.activeSelf) InitMissionPanel();
        // Vai fechar
        else CleanMissionPanel();

        // Ação de abrir ou fechar
        _missionCanvas.SetActive(!_missionCanvas.activeSelf);

        // Se aberto
        if (_missionCanvas.activeSelf) _mc.FocusAtFirstElement();
    }

    /// <summary>
    /// Instantiate all buttons
    /// </summary>
    private void InitMissionPanel()
    {
        List<Mission> missionList = _aus.GetMissionsRegistered();

        foreach (Mission mission in missionList) {
            if (mission.IsCompleted) InstantiateMissionButton(mission, _completedButton);
            else InstantiateMissionButton(mission, _missionButton);
        }
    }

    /// <summary>
    /// Remove all existent buttons on panel
    /// </summary>
    private void CleanMissionPanel()
    {
        foreach (Transform child in _buttonGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Instantiate mission buttons
    /// </summary>
    private void InstantiateMissionButton(Mission mission, GameObject _button)
    {
        GameObject tmpGo = Instantiate(_button, _buttonGroup.transform);
        MissionButton mb = tmpGo.GetComponent<MissionButton>();

        mb.MissionTitle = mission.MissionTitle;
        mb.ShortDescription = mission.ShortDescription;
        mb.FullDescription = mission.FullDescription;
        mb.Thumbnail = mission.Thumbnail;
        mb.Image = mission.Image;
        mb.SetIsCompleted(mission.IsCompleted);
        mb.InitializeButton();
    }
}
