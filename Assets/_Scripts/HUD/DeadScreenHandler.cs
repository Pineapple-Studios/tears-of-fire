using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadScreenHandler : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Dead Pannel")]
    private GameObject _deadPanel;
    [SerializeField]
    private Button _restartButton;

    private void OnEnable()
    {
        PlayerProps.onPlayerDead += ActiveDeadScreen;
        _restartButton.onClick.AddListener(RestartScene);
    }

    private void OnDisable()
    {
        PlayerProps.onPlayerDead -= ActiveDeadScreen;
        _restartButton.onClick.RemoveListener(RestartScene);
    }

    private void ActiveDeadScreen()
    {
        _deadPanel.SetActive(true);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
