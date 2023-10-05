using UnityEngine;

public class FOVChanger : MonoBehaviour
{
    [Header("Casting")]
    [SerializeField]
    private LayerMask _playerMask;

    [Header("Props")]
    [SerializeField]
    private float _targetFieldOfView = 95f;
    [SerializeField]
    private float _transitionSpeed = 15f;

    private bool _mustUpdate = false;
    private bool _isBiggerThenCurrent = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((((1 << collision.gameObject.layer) & _playerMask) != 0) && LevelDataManager.Instance.MainCamera != null)
        {
            _isBiggerThenCurrent = _targetFieldOfView > LevelDataManager.Instance.MainCamera.m_Lens.FieldOfView;
            // Debug.Log($"{_targetFieldOfView}; {LevelDataManager.Instance.MainCamera.m_Lens.FieldOfView}");
            if (_targetFieldOfView != LevelDataManager.Instance.MainCamera.m_Lens.FieldOfView) _mustUpdate = true;
        }
    }

    private void FixedUpdate()
    {
        if (_targetFieldOfView == LevelDataManager.Instance.MainCamera.m_Lens.FieldOfView) _mustUpdate = false;

        if (_mustUpdate)
        {
            float step = _isBiggerThenCurrent ? _transitionSpeed : _transitionSpeed * -1;
            LevelDataManager.Instance.MainCamera.m_Lens.FieldOfView += Time.fixedDeltaTime * step;

            if (_isBiggerThenCurrent && _targetFieldOfView < LevelDataManager.Instance.MainCamera.m_Lens.FieldOfView) SetFieldOfView();
            if (!_isBiggerThenCurrent && _targetFieldOfView > LevelDataManager.Instance.MainCamera.m_Lens.FieldOfView) SetFieldOfView();
        }
    }

    private void SetFieldOfView() {
        LevelDataManager.Instance.MainCamera.m_Lens.FieldOfView = _targetFieldOfView;
    }
}
