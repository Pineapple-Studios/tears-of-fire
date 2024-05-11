using UnityEngine;

public class DoorLockedAnimationTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject _sceneKey;

    public void OpenDoor()
    {
        gameObject.SetActive(false);
        LevelDataManager.Instance.SetKwyRoomKey(false);
    }

    public void PutKey()
    {
        _sceneKey.SetActive(true);
        CinemachineShakeManager.Instance.ShakeCamera(2, 2);
        RumbleManager.instance.RumbleOpenDoor();
    }
}
