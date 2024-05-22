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
        if (_pih != null) _pih.KeyMissionDown += OpenMissionMenu;
    }

    private void OnDisable()
    {
        if (_pih != null) _pih.KeyMissionDown -= OpenMissionMenu;
    }

    private void OpenMissionMenu()
    {
        if (_isDialogRunning) return;

        if (!_missionCanvas.activeSelf)
        {
            InitMissionPanel();
        }

        _missionCanvas.SetActive(!_missionCanvas.activeSelf);

        if (_missionCanvas.activeSelf) _mc.FocusAtFirstElement();
    }

    private void InitMissionPanel()
    {
        _aus.UpdateMissions();
        List<Mission> missionList = _aus.GetMissionsRegistered();
        if (missionList.Count == _buttonGroup.transform.childCount)
        {
            ReloadAllButtons(missionList);
            return;
        }

        foreach (Mission mission in missionList) { InstantiateMissionButton(mission, _missionButton); }
    }

    private void ReloadAllButtons(List<Mission> missionList)
    {
        MissionButton[] btns = _buttonGroup.GetComponentsInChildren<MissionButton>();
        foreach (MissionButton mb in btns)
        {
            foreach (Mission mission in missionList)
            {
                if (mb.MissionTitle == mission.MissionTitle)
                {
                    if (mission.IsCompleted)
                    {
                        RemoveAndInstantiate(mission, mb.gameObject);
                    }
                    else
                    {
                        mb.MissionTitle = mission.MissionTitle;
                        mb.ShortDescription = mission.ShortDescription;
                        mb.FullDescription = mission.FullDescription;
                        mb.Thumbnail = mission.Thumbnail;
                        mb.Image = mission.Image;
                        mb.SetIsCompleted(mission.IsCompleted);
                        mb.InitializeButton();
                    }
                }
            }
        }

        if (_missionCanvas.activeSelf) _mc.FocusAtFirstElement();
    }

    private void RemoveAndInstantiate(Mission mission, GameObject gameObject)
    {
        Destroy(gameObject);
        InstantiateMissionButton(mission, _completedButton);
    }

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
