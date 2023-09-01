using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    public Transform Target;
    [SerializeField]
    private Vector3 OffsetCam = new Vector3(0, 2, 12);

    private Transform _camera;

    private void Start()
    {
        _camera = GetComponent<Transform>();
    }

    void Update()
    {
        if (Target == null) return;

        _camera.position = new Vector3(Target.position.x, 0, Target.position.z);
        _camera.position -= Target.forward * OffsetCam.z;
        _camera.rotation = Quaternion.identity;

        if (Target.position.y > 3)
        {
            _camera.position  = Target.up;
        }


            if (Target.position.x > 3)
        {     
            _camera.position += Target.right * OffsetCam.x;  
        }

        if (Target.position.x < 3)
        {
            _camera.position = new Vector3(0, 0, _camera.position.z);
        }
    }
}
