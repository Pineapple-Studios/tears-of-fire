using UnityEngine;

public class YkeEvent : MonoBehaviour
{
    private PlayerProps _pp;

    private void Awake()
    {
        _pp = FindAnyObjectByType<PlayerProps>();
    }

    private void OnEnable()
    {
        NPC.FinishNPCDialog += OnFinishDialog;
    }

    private void OnDisable()
    {
        NPC.FinishNPCDialog -= OnFinishDialog;
    }

    private void OnFinishDialog(NPC npc)
    {
        if (npc.NpcName != "Yke") return;

        if (!LevelDataManager.Instance.GetKwyRoomKey()) FeedbackAndDebugManager.Instance.ToggleKwyKey();
        if (_pp != null && _pp.GetCurrentMaxLife() < 80f) _pp.AddMaxLife(1);
        gameObject.SetActive(false);
    }
}
