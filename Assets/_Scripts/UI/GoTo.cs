using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoTo : MonoBehaviour
{
    private Button _btn;
    public string SceneName;

    private void Awake()
    {
        _btn = GetComponent<Button>();
    }

    void OnEnable()
    {
        _btn.onClick.AddListener(GoToScene);
    }

    private void OnDisable()
    {
        _btn.onClick.RemoveListener(GoToScene);
    }

    private void GoToScene()
    {
        if (SceneName == null) Debug.Log("You must set the scene name");

        SceneManager.LoadScene(SceneName);
    }
}
