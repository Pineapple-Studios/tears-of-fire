using UnityEngine;

public class VFXBreakableBlock : MonoBehaviour
{
    private const string SHAKE_CLIP = "clip_shake";

    private SpriteRenderer _breakableSprite;
    private Animator _anim;

    void Start()
    {
        _breakableSprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    public void ColorChange(int hitsToBreak, int counter)
    {
        if (_breakableSprite == null) return;

        float percent = 1f / hitsToBreak;
        float colorValue = percent * counter; 

        colorValue = Mathf.Clamp01(colorValue);
        _breakableSprite.color = new Color(1, 1 - colorValue, 1 - colorValue, 1);
    }

    public void ShakeSprite()
    {
        if (_anim == null) return;

        _anim.Play(SHAKE_CLIP);
    }
}
