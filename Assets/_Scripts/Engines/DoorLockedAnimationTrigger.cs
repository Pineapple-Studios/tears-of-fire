using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLockedAnimationTrigger : MonoBehaviour
{
    public void OpenDoor()
    {
        gameObject.SetActive(false);
        LevelDataManager.Instance.SetKwyRoomKey(false);
    }
}
