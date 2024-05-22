using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MissionController : MonoBehaviour
{
    private Hashtable _missions = new Hashtable();

    private void Start()
    {
        FocusAtFirstElement();

        _missions.Add("Title", "Tutorial");
    }

    public void FocusAtFirstElement()
    {
        Button[] btns = GetComponentsInChildren<Button>();
        EventSystem.current.SetSelectedGameObject(btns[0].gameObject);
    }
}
