using UnityEngine;

public class FOVChanger : MonoBehaviour
{
    [Header("Casting")]
    [SerializeField]
    private LayerMask _playerMask;

    [Header("Props")]
    [SerializeField]
    private float _targetFieldOfView = 95f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & _playerMask) != 0) && LevelDataManager.Instance.MainCamera != null)
        {
            SetFieldOfView();
        }
    }

    private void SetFieldOfView() {
        LevelDataManager.Instance.MainCamera.m_Lens.FieldOfView = _targetFieldOfView;
    }
}
