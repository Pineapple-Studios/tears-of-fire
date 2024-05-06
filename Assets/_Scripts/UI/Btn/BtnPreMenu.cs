using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnPreMenu : MonoBehaviour
{
    [SerializeField] Button btn;

    private void Start()
    {
        CursorInvisible.Instance.InvisibleCursor();
    }

    private void OnEnable()
    {
        btn.onClick.AddListener(GoToScene);
    }

    private void OnDisable()
    {
        btn.onClick.RemoveListener(GoToScene);
    }

    public void GoToScene()
    {
        if (SceneManager.GetActiveScene().name == "SelectScreen")
        {
            SceneManager.LoadScene("BrightnessScreen");
        }
        else if (SceneManager.GetActiveScene().name == "BrightnessScreen")
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if (SceneManager.GetActiveScene().name == "BeforeGame")
        {
            SceneManager.LoadScene("Tutorial");
        }
    }
}
