using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HiddenPath : MonoBehaviour
{
    [Header("Casting")]
    [SerializeField]
    private LayerMask _playerLayer;
    [SerializeField]
    private TilemapRenderer _componentToEnable;
    [SerializeField] private GameObject[] _secretPaths;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & _playerLayer) != 0))
        {
            _componentToEnable.enabled = false;

            foreach (GameObject path in _secretPaths)
            {
                if (path != null) path.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & _playerLayer) != 0))
        {
            _componentToEnable.enabled = true;
            foreach (GameObject path in _secretPaths)
            {
                if (path != null) path.SetActive(true);
            }
        }
    }
}
