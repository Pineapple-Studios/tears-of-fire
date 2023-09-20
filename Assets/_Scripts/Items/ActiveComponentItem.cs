using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveComponentItem : MonoBehaviour
{
    [Header("Casting")]
    [SerializeField]
    private LayerMask _playerLayer;
    [SerializeField]
    private MonoBehaviour _componentToEnable;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & _playerLayer) != 0))
        {
            _componentToEnable.enabled = true;
            // TODO: Devo salvar a informação que o componente foi capturado na memória do jogo

            Destroy(gameObject);
        }
    }
}
