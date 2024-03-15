using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoToCheckpoint : MonoBehaviour
{
    public string CheckpointName;
    public Vector3 Pos;

    private Button _btn;
    private TMP_Text _text;

    private void Awake()
    {
        _btn = GetComponent<Button>();
        _text = GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        if (_text == null || _text.text == CheckpointName) return;

        _text.text = CheckpointName;
    }

    void OnEnable()
    {
        _btn.onClick.AddListener(Go);
    }

    private void OnDisable()
    {
        _btn.onClick.RemoveListener(Go);
    }

    private void Go()
    {
        PlayerController pc = FindAnyObjectByType<PlayerController>();
        GameObject go = FindAnyObjectByType<PlayerController>().gameObject;
        if (go == null) return;

        Animator anim = go.GetComponentInChildren<Animator>();
        pc.Respawn();

        anim.Rebind();
        anim.Update(0f);
        LevelDataManager.Instance.RespawnToCheckpoint(go, Pos);
    }
}
