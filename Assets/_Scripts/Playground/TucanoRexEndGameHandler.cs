using System;
using UnityEngine;

public class TucanoRexEndGameHandler : MonoBehaviour
{
    [SerializeField]
    private TucanoRex _kwy;
    [SerializeField]
    private GameObject _cvFeedback;
    [SerializeField]
    private GameObject[] _elementsToDeactive;
    [SerializeField]
    private GameObject[] _elementsToActive;

    private PlayerInputHandler _playerInputHandler;

    private void Awake()
    {
        _playerInputHandler = FindAnyObjectByType<PlayerInputHandler>();
    }

    private void OnEnable()
    {
        TucanoRexProps.onTucanoRexDead += OnTucanoRexDead;
        if (_playerInputHandler != null)
        {
            _playerInputHandler.KeyNPCInteractionDown += RoolBackToGame;
        }
    }

    private void OnDisable()
    {
        TucanoRexProps.onTucanoRexDead -= OnTucanoRexDead;
        if (_playerInputHandler != null)
        {
            _playerInputHandler.KeyNPCInteractionDown -= RoolBackToGame;
        }
    }

    private void OnTucanoRexDead(GameObject obj)
    {
        Time.timeScale = 0f;
        _cvFeedback.SetActive(true);
        DeactiveElements();
    }

    private void DeactiveElements()
    {
        foreach (GameObject element in _elementsToDeactive)
        {
            element.SetActive(false);
        }
    }

    private void RoolBackToGame()
    {
        if (!_kwy.IsStarted()) return;

        Time.timeScale = 1f;
        _cvFeedback.SetActive(false);
        ActiveElements();
        // Enable dash
        LevelDataManager.Instance.SetDashState(true);
    }

    private void ActiveElements()
    {
        foreach(GameObject element in _elementsToActive)
        {
            element.SetActive(true);
        }
    }
}
