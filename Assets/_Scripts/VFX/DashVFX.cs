using UnityEngine;

public class DashVFX : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    
    private Texture _texture;
    private ParticleSystem _particleSystem;
    private Material _material;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        _texture = _spriteRenderer.sprite.texture;
        _particleSystem.textureSheetAnimation.SetSprite(0, _spriteRenderer.sprite);
    }

}
