using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticHandler : MonoBehaviour
{
    private const string STEP_STATE_NAME = "step";

    [SerializeField]
    private int _totalSteps;
    [SerializeField]
    private Animator _platformAnimator;
    [SerializeField]
    private Platform _platform;
    [SerializeField]
    private float _cooldown = 3f;

    private float _timer = 0f;
    private int _tmpCurrentStep = 0;

    private void OnEnable()
    {
        LevelDataManager.onRestartElements += StartDeath;
    }

    private void OnDisable()
    {
        LevelDataManager.onRestartElements -= StartDeath;
    }

    private void Update()
    {
        _tmpCurrentStep = GetCurrentStep();
        
        if (_tmpCurrentStep == 0 || _platform.anim_isAnimating)
        {
            _timer = 0f;
            return;
        }

        if (_timer < _cooldown)
        {
            _timer += Time.deltaTime;
        }
        else 
        {
            OnPreviousStep();
            _timer = 0f;
        }
    }

    private void StartDeath()
    {
        ResetPuzzle();
    }

    private int GetCurrentStep()
    {
        return _platformAnimator.GetInteger(STEP_STATE_NAME);
    }

    /// <summary>
    /// Must be used to restart the game
    /// </summary>
    public void ResetPuzzle()
    {
        _platformAnimator.Rebind();
        _platformAnimator.Update(0f);
        _platformAnimator.SetInteger(STEP_STATE_NAME, 0);
    }
    
    public void OnNextStep() {
        _platform.OnStartMovementByTrigger();

        int step = GetCurrentStep();
        if (step >= _totalSteps || _platform.anim_isAnimating)
        {
            FeedbackManagerHandler.Instance.NegativeFeedback();
            return;
        }
        
        

        _timer = 0f;
        _platformAnimator.SetInteger(STEP_STATE_NAME, step + 1);
    }

    public void OnPreviousStep() {
        _platform.OnStartMovementByTrigger();

        int step = GetCurrentStep();
        if (step == 0 || _platform.anim_isAnimating)
        {
            FeedbackManagerHandler.Instance.NegativeFeedback();
            return;
        }

        _platformAnimator.SetInteger(STEP_STATE_NAME, step - 1);
    }
}
