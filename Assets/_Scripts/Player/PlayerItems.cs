using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    private LevelDataManager _ld;

    private void Start()
    {
        _ld = LevelDataManager.Instance;
    }

    public void GettingKwyKey() => _ld.SetKwyRoomKey(true);
    public void LoseKwyKey() => _ld.SetKwyRoomKey(false);
    public bool HasKwyRoomKey() => _ld.GetKwyRoomKey();
}
