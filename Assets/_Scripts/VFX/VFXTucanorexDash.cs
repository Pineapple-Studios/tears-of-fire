using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXTucanorexDash : MonoBehaviour
{
    [SerializeField]
    private GameObject _dashPrefab;
    [SerializeField]
    private int _qtdProjection;
    [SerializeField]
    private Vector3 _gap;
    [SerializeField]
    [Tooltip("Velocity to propagete FX")]
    private float _speed = 10;
    [SerializeField]
    private SpriteRenderer _spriteSource;

    [Header("Tucanorex Props")]
    [SerializeField]
    private Transform _instancesParent;
    [SerializeField]
    private Transform _shadowMovement;

    private List<GameObject> _instancesStack = new List<GameObject>();

    private Vector3 _startEffectPosition;
    private bool _isStarted = false;
    private bool _isStoppping = false;
    private int _currentCount = 0;
    private SpriteRenderer _tmpSpriteRenderer;
    private GameObject _tmpElement; // Reusable variable

    private void Start()
    {
        CreatePooling();
    }

    /// <summary>
    /// Creating instances pooling with right colorized sprite FX
    /// </summary>
    private void CreatePooling()
    {
        for (int i = 0; i < _qtdProjection; i++)
        {
            _tmpElement = Instantiate(_dashPrefab, _instancesParent);
            _tmpElement.transform.localPosition = Vector3.zero;
            _tmpElement.SetActive(false);
            SpriteRenderer tmpSpriteRenderer = _tmpElement.GetComponent<SpriteRenderer>();
            tmpSpriteRenderer.color = new Color(0, 0, 256, (_qtdProjection - i) * 0.1f);
            _instancesStack.Add(_tmpElement);
        }
    }

    private void Update()
    {
        UpdateTranslationBasedOnShadow();

        if (_isStoppping)
        {
            StopEffect();
            StopAllEffects();
            return;
        }

        if (_instancesStack.Count > 0)
        {
            UpdateSprites();
            UpdatePosition();
        }

        if (!_isStarted) return;

        ShowDash();
    }

    private void UpdateTranslationBasedOnShadow()
    {
        transform.position = _shadowMovement.position;
    }

    /// <summary>
    /// Update stack instances sprites to be the same of _spriteSource element
    /// </summary>
    private void UpdateSprites()
    {
        for (int i = 0; i < _qtdProjection; i++)
        {
            _tmpElement = _instancesStack[i];
            _tmpSpriteRenderer = _tmpElement.GetComponent<SpriteRenderer>();
            _tmpSpriteRenderer.sprite = _spriteSource.sprite;
            _tmpSpriteRenderer.sortingOrder = _spriteSource.sortingOrder - 1 - i;
            _tmpSpriteRenderer.flipX = _spriteSource.flipX;
        }
    }

    /// <summary>
    /// Align FX to parent sprite in movement
    /// </summary>
    private void UpdatePosition()
    {
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < _qtdProjection; i++)
        {
            GameObject instance = _instancesStack[i];
            if (!instance.activeSelf) continue;

            Vector3 appliedGap = IsGoingToRight() ?
                pos - (_gap * (i + 1)) :
                pos + (_gap * (i + 1));

            Vector3 local = new Vector3(instance.transform.localPosition.x, 0, 0);

            instance.transform.localPosition = Vector3.Lerp(
                local, 
                appliedGap, 
                Time.deltaTime * _speed
            );
        }
    }

    private void StopEffect()
    {
        for (int i = 0; i < _qtdProjection; i++)
        {
            GameObject tmpGb = _instancesStack[i];
            tmpGb.transform.localPosition = Vector3.Lerp(
                tmpGb.transform.localPosition,
                Vector3.zero,
                Time.deltaTime * (_speed / 2)
            );

            if (NearToRemove(tmpGb.transform)) tmpGb.SetActive(false);
        }
    }

    private void StopAllEffects()
    {
        int tmpCount = 0;
        for (int i = 0; i < _qtdProjection; i++)
        {
            GameObject tmpGb = _instancesStack[i];
            if (NearToRemove(tmpGb.transform)) tmpCount++;
        }

        if (tmpCount == _qtdProjection) _isStoppping = false;
    }

    private bool IsGoingToRight() => _startEffectPosition.x < transform.position.x;

    private bool NearToRemove(Transform trans) => (int)Vector3.Distance(trans.localPosition, Vector3.zero) <= 0;

    private void ShowDash()
    {
        float gap = 
            Vector3.Distance(_startEffectPosition, _startEffectPosition + _gap);

        if (Vector3.Distance(_startEffectPosition, transform.position) >= gap)
        {
            _instancesStack[_currentCount].SetActive(true);
            _instancesStack[_currentCount].transform.position = transform.position;
            _startEffectPosition = transform.position;
            _currentCount++;
        }


        if (_currentCount == _qtdProjection)
        {
            _isStarted = false;
        }
    }

    public void TriggerDash()
    {
        Debug.Log("Init");
        _startEffectPosition = transform.position;
        _isStarted = true;
    }

    public void TriggerStopDash()
    {
        Debug.Log("End");
        _currentCount = 0;
        _isStoppping = true;
        _isStarted = false;
    }
}
