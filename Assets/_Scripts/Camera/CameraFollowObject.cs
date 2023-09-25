using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform _playerTransform;

    [Header("Flip Rotation Stats")]
    [SerializeField]
    private float _flipRotationTime = 0.5f;

    private Coroutine _turnCoroutine;

    private PlayerController _player;

    private bool _isFacingRight;

    private void Awake()
    {
        _player = _playerTransform.gameObject.GetComponent<PlayerController>();
        _isFacingRight = _player.IsFacingRight;
    }

    private void Update()
    {
        if (_playerTransform == null) return;

        transform.position = Vector3.Lerp(transform.position, _playerTransform.position, Time.deltaTime);
    }

    public void CallTurn()
    {
        _turnCoroutine = StartCoroutine(FlipYLerp());
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmmount = DetermineEndRotation();
        float yRotation = 0f;

        float elepsedTime = 0f;
        while (elepsedTime < _flipRotationTime)
        {
            elepsedTime += Time.deltaTime;
            yRotation = Mathf.Lerp(startRotation, endRotationAmmount, (elepsedTime / _flipRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        _isFacingRight = !_isFacingRight;

        if (_isFacingRight) return 180f;
        else return 0f;
    }

    public void SetFollowTo(Transform trans)
    {
        _playerTransform = trans;
    }
}
