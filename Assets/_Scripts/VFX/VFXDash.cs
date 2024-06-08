using System.Collections.Generic;
using UnityEngine;

public class VFXDash : MonoBehaviour
{
    [SerializeField]
    private GameObject _dashPrefab;
    [SerializeField]
    private int _qtdProjection;
    [SerializeField]
    private Vector3 _gap; 
    [SerializeField]
    private float _speed = 10;
    [SerializeField]
    private SpriteRenderer _spriteSource;

    private List<GameObject> _instancesStack = new List<GameObject>();
    private GameObject _tmpElement;

    private Vector3 _startEffectPosition;
    private bool _isStarted = false;
    private bool _isStoppping = false;
    private int _currentCount = 0;
    private SpriteRenderer _tmpSpriteRenderer;

    private void Start()
    {
        CreatePooling();
    }

    private void Update()
    {
        if (_isStoppping)
        {
            StopEffect();
            StopAllEffects();
            return;
        }

        if (_instancesStack.Count > 0)
        {
            UpdatePosition();
            UpdateSprites();
        }

        if (!_isStarted) return;

        InstantiateDash();
    }

    private void CreatePooling()
    {
        for (int i = 0; i < _qtdProjection; i++)
        {
            _tmpElement = Instantiate(_dashPrefab, transform);
            _tmpElement.SetActive(false);
            SpriteRenderer tmpSpriteRenderer = _tmpElement.GetComponent<SpriteRenderer>();
            tmpSpriteRenderer.color = new Color(0,0,256, (_qtdProjection - i) * 0.1f);
            _instancesStack.Add(_tmpElement);
        }
    }

    private void UpdatePosition()
    {
        for (int i = 0; i < _qtdProjection; i++)
        {
            GameObject tmpGb = _instancesStack[i];
            if (!tmpGb.activeSelf) continue;

            Vector3 appliedGap = IsGoingToRight() ?
                transform.position - (_gap * (i + 1)) :
                transform.position + (_gap * (i + 1));

            tmpGb.transform.position = Vector3.Lerp(
                tmpGb.transform.position, appliedGap, Time.deltaTime * _speed
            );
        }
    }

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

    private void InstantiateDash()
    {
        if (
            Vector3.Distance(_startEffectPosition, transform.position) >= 
            Vector3.Distance(_startEffectPosition, _startEffectPosition + _gap)
        )
        {
            _instancesStack[_currentCount].SetActive(true);
            _instancesStack[_currentCount].transform.position = transform.position;
            _startEffectPosition = transform.position;
            _currentCount++;
        }


        if(_currentCount == _qtdProjection)
        {
            _isStarted = false;
        }
    }

    private void StopEffect()
    {
        for (int i = 0; i < _qtdProjection; i++)
        {
            GameObject tmpGb = _instancesStack[i];
            tmpGb.transform.position = Vector3.Lerp(
                tmpGb.transform.position,
                transform.position,
                Time.deltaTime * _speed
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

    private bool NearToRemove(Transform trans)
    {
        return (int) Vector3.Distance(trans.position, transform.position) <= 0;
    }

    private bool IsGoingToRight()
    {
        return _startEffectPosition.x < transform.position.x;
    }

    public void TriggerStopDash()
    {
        _currentCount = 0;
        _isStoppping = true;
        _isStarted = false;
    }

    public void TriggerDash()
    {
        _startEffectPosition = transform.position;
        _isStarted = true;
    }
}
