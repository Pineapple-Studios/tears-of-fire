using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelDoor : MonoBehaviour
{
    [SerializeField]
    private Vector2 _forceApplied;
    [Tooltip("In seconds")]
    [SerializeField]
    private float _delayToGo = 2;
    [SerializeField]
    private string _nextLevel;

    private bool _mustStart = false;
    private Rigidbody2D _playerRb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            PlayerController _pc = collision.gameObject.GetComponent<PlayerController>();
            _pc.DisableInput();
            _mustStart = true;
            StartCoroutine(GoToNextLevel());
        }
    }

    private IEnumerator GoToNextLevel()
    {
        yield return new WaitForSeconds(_delayToGo);
        SceneManager.LoadScene(_nextLevel);
    }

    private void Update()
    {
        if (!_mustStart) return;

        _playerRb.gravityScale = 0f;
        _playerRb.velocity += _forceApplied * Time.deltaTime;
    }

}
