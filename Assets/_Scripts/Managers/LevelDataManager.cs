using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class LevelDataManager : MonoBehaviour
{
    public static Action onRestartElements;

    [SerializeField]
    private LevelDataScriptableObject _levelData;

    [Header("Camera")]
    [SerializeField]
    public CinemachineVirtualCamera MainCamera;

    [Header("Screen FX")]
    [SerializeField]
    private GameObject _deathTransition;
    [SerializeField]
    private float _waitTransitionSeconds;

    public GameObject CurrentPlayerObject;

    public static LevelDataManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _deathTransition.SetActive(false);
    }


    private void OnEnable()
    {
        Checkpoint.NewCheckpoint += SetLastCheckpoint;
    }

    private void OnDisable()
    {
        Checkpoint.NewCheckpoint -= SetLastCheckpoint;
    }

    private void Update()
    {
        HandlePlayerEngines();
    }

    private void HandlePlayerEngines()
    {
        // Ativando dash de acordo com o estado do controlador
        var dash = FindObjectOfType<PlayerDash>();
        if (dash != null && dash.enabled != _levelData.hasDash)
        {
            dash.enabled = _levelData.hasDash;
        }
    }

    public void Respawn(GameObject obj)
    {
        _levelData.TimesDied += 1;

        // TODO: Criar transição de morte do SVART

        obj.SetActive(false);

        StartCoroutine(HandleRespawnAnimationPlayer(obj));
    }

    private IEnumerator HandleRespawnAnimationPlayer(GameObject player)
    {
        _deathTransition.SetActive(true);
        yield return new WaitForSeconds(_waitTransitionSeconds / 2);

        // Mudando a localização do jogador quando a tela está escura
        player.transform.position = _levelData.lastCheckpoint;
        // Mostrando a vida
        player.GetComponentInChildren<PlayerProps>().FullHeal();
        onRestartElements();

        yield return new WaitForSeconds(_waitTransitionSeconds / 2);
        _deathTransition.SetActive(false);

        player.SetActive(true);
        player.GetComponentInChildren<PlayerAnimationController>().StartRespawn();
    }

    private void SetLastCheckpoint(Vector3 trans)
    {
        _levelData.lastCheckpoint = trans;
    }

    public void SetDashState(bool state)
    {
        _levelData.hasDash = state;
    }

    public bool GetDashStatus()
    {
        return _levelData.hasDash;
    }
}
