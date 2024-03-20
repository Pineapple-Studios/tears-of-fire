using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineSwitcher : MonoBehaviour
{
    [Header("Casting")]
    [SerializeField]
    private LayerMask _playerMask;
    [SerializeField]
    private Animator _anim;

    [Header("Props")]
    [SerializeField]
    private ECamAvailable _goToCam;

    private const string PLAYER_CAM_STATE = "PlayerCamera";
    private const string SCEARIO_CAM_STATE = "ScenarioCamera";
    private const string BOSS_CAM_STATE = "BossCamera";
    private const string YAMSE_CAM_STATE = "YamseCamera";
    private const string GRAVURA_CAM_STATE_A = "GravuraCamera_A";
    private const string GRAVURA_CAM_STATE_B = "GravuraCamera_B";
    private const string GRAVURA_CAM_STATE_C = "GravuraCamera_C";


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & _playerMask) != 0) && LevelDataManager.Instance.MainCamera != null)
        {
            SwitchCam();
        }
    }

    private void SwitchCam()
    {
        if (_goToCam == LevelDataManager.Instance.GetCamState()) return;

        switch (_goToCam)
        {
            case ECamAvailable.SCENARIO:
                _anim.Play(SCEARIO_CAM_STATE);
                break;
            case ECamAvailable.BOSS:
                _anim.Play(BOSS_CAM_STATE);
                break;
            case ECamAvailable.YAMSE:
                _anim.Play(YAMSE_CAM_STATE);
                break;
            case ECamAvailable.GRAVURA_A:
                _anim.Play(GRAVURA_CAM_STATE_A);
                break;
            case ECamAvailable.GRAVURA_B:
                _anim.Play(GRAVURA_CAM_STATE_B);
                break;
            case ECamAvailable.GRAVURA_C:
                _anim.Play(GRAVURA_CAM_STATE_C);
                break;
            case ECamAvailable.PLAYER:
            default:
                _anim.Play(PLAYER_CAM_STATE);
                break;
        }

        LevelDataManager.Instance.SetNewCamState(_goToCam);
    }
}

public enum ECamAvailable
{
    PLAYER,
    SCENARIO,
    BOSS,
    YAMSE,
    GRAVURA_A,
    GRAVURA_B,
    GRAVURA_C
}