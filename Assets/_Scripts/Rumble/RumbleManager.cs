using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class RumbleManager : MonoBehaviour
{
    public static RumbleManager instance;

    private Gamepad pad;

    private Coroutine stopRumbleCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    /// <summary>
    /// Vibração do controle
    /// </summary>
    /// <param name="lowFrequency">lowFreq motor</param>
    /// <param name="highFrequency">highFreq motor</param>
    /// <param name="duration">Variação tempo segundos</param>
    public void RumblePulse(float lowFrequency, float highFrequency, float duration)
    {
        pad = Gamepad.current;

        if (pad != null)
        {
            pad.SetMotorSpeeds(lowFrequency, highFrequency);
        }

        stopRumbleCoroutine = StartCoroutine(StopRumble(duration, pad));
    }

    public void RumblePlayerDamage() {
        RumblePulse(0.55f, 2f, 0.25f);
    }

    public void RumbleEnemyDamage() {
        RumblePulse(0.25f, 1f, 0.25f);
    }

    public void RumbleBossDamage() {
        RumblePulse(0.45f, 1.5f, 0.25f);
    }

    public void RumbleBossDead() {
        RumblePulse(1f, 2f, 0.5f);
    }

    private IEnumerator StopRumble(float duration, Gamepad pad)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        
        }

        if (pad != null) pad.SetMotorSpeeds(0f, 0f);
    }
}
