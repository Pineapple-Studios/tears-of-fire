using UnityEngine;

public class CameraVFX : MonoBehaviour
{
    [SerializeField]
    private Transform _camera;
    [SerializeField]
    private Vector3 _offset;
    [SerializeField]
    private Transform _player;

    // Update is called once per frame
    void Update()
    {
        _camera.position = _player.position + _offset;
    }
}
