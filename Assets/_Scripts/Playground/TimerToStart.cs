using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerToStart : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _timerToStart;
    [SerializeField]
    private GameObject _timerPanel;

    private int _maxTimerCount = 6;
    private int _timerCount = 6;

    public void UpdateTimerToStart()
    {
        if (_timerCount == 0)
        {
            _timerCount = _maxTimerCount;
            Time.timeScale = 1;
            _timerPanel.SetActive(false);
            return;
        }

        _timerCount--;
        _timerToStart.text = _timerCount.ToString();
    }
}
