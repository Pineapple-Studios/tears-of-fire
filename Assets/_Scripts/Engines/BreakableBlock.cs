using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock: MonoBehaviour
{
    [Header("Props")]
    [SerializeField]
    private int _hitsToBreak = 3;
    [SerializeField]
    private List<GameObject> _blockDepedencies = new List<GameObject> { };

    private BoxCollider2D _col;
    private int _counter = 0;

    private void Start()
    {
        SetColliderAccordingOfSprite();
    }

    /// <summary>
    /// Executa um hit no bloco quebrável e verifica se devemos destruílo
    /// </summary>
    public void HitWall()
    {
        _counter++;
        Debug.Log("Block code hit");
        if (_counter == _hitsToBreak)
        {
            GameObject parent = gameObject.transform.parent.gameObject;
            DestroyDependencies();
            Destroy(parent);
        }
    }

    /// <summary>
    /// Destroi a lista de elementos na dependencia desse bloco
    /// </summary>
    private void DestroyDependencies()
    {
        if (_blockDepedencies.Count == 0) return;

        foreach(GameObject obj in _blockDepedencies)
        {
            Destroy(obj);
        }
    }

    /// <summary>
    /// Busca o elemento que tem o sprite e adiciona o collider para que o collider
    /// possua o mesmo tamanho do sprite exibido;
    /// </summary>
    private void SetColliderAccordingOfSprite()
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        SpriteRenderer spriteRenderer = parent.GetComponentInChildren<SpriteRenderer>();
        GameObject element = spriteRenderer.gameObject;
        _col = element.AddComponent<BoxCollider2D>();
    }
}
