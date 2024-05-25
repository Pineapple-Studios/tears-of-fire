using System;
using UnityEngine;

public class StartBoss : MonoBehaviour
{
    public static Action OnStartedBoss;

    [SerializeField]
    private TucanoRex _kwy;

    private StartBossGroupHandler _groupHandler;

    private void Start()
    {
        _groupHandler = GetComponentInParent<StartBossGroupHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (_groupHandler != null)
            {
                _groupHandler.FirstInteractionActionDone();
                _groupHandler.ActiveScenarioBlockers();
            }

            _kwy.StartTucanoRex();
            OnStartedBoss();
            gameObject.SetActive(false);
        }
    }
}
