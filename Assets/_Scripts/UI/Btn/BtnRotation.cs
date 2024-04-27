using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnRotation : MonoBehaviour
{
    
    [SerializeField] Button _goToScreenButton;
    //[SerializeField] public Animator transition;

    private void Start()
    {
        Cursor.visible = true;
    }

    private void OnEnable()
    {
        _goToScreenButton.onClick.AddListener(OnTransition);
    }

    private void OnDisable()
    {
        _goToScreenButton.onClick.RemoveListener(OnTransition);
    }

    public void OnTransition()
    {
        SceneManager.LoadScene("FinishScreen");
    }
}
