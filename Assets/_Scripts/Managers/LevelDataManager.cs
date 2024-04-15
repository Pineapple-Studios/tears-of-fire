using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using static LevelDataScriptableObject;

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
    private ECamAvailable _currentSelectedCam = ECamAvailable.PLAYER;
    private GameObject _player;

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

        _levelData.EnemiesDead.Clear();
        // Set last checkpoint as the initial position
        _levelData.lastCheckpoint = new Vector3(14.5f,4.3f,0);
        _player = FindAnyObjectByType<PlayerController>().gameObject;
    }

    private void Start()
    {
        InitStates();
    }

    private void InitStates()
    {
        _deathTransition.SetActive(false);
        SetKwyRoomKey(false);
        SetDashState(false);
        _player.transform.position = _levelData.lastCheckpoint;
        // _player.GetComponent<PlayerController>().Flip();
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

        obj.SetActive(false);

        StartCoroutine(HandleRespawnAnimationPlayer(obj));
    }

    private IEnumerator HandleRespawnAnimationPlayer(GameObject player)
    {
        _deathTransition.SetActive(true);
        yield return new WaitForSeconds(_waitTransitionSeconds / 2);

        // Mudando a localização do jogador quando a tela está escura
        player.transform.position = _levelData.lastCheckpoint;
        // Reiniciando inimigos
        RespawnEnemies();
        // Mostrando a vida
        player.GetComponentInChildren<PlayerProps>().FullHeal();
        onRestartElements();

        yield return new WaitForSeconds(_waitTransitionSeconds / 2);
        _deathTransition.SetActive(false);

        player.SetActive(true);
        player.GetComponentInChildren<PlayerAnimationController>().StartRespawn();
    }

    private void RespawnEnemies()
    {
        foreach (EnemyDead enemy in _levelData.EnemiesDead)
        {
            GameObject obj = enemy.GameObject;
            obj.transform.position = enemy.Position;
            obj.SetActive(true);
            obj.GetComponentInChildren<IWalkStart>().ResetWalk();
            obj.GetComponentInChildren<Enemy>().EnableColliderOnRespawn();
            obj.GetComponentInChildren<Enemy>().ResetEnemy();
        }

        _levelData.EnemiesDead.Clear();
    }

    private void SetLastCheckpoint(Vector3 trans)
    {
        _levelData.lastCheckpoint = trans;
    }

    public void SetKwyRoomKey(bool state)
    {
        _levelData.hasKwyRoomKey = state;
    }
    public bool GetKwyRoomKey()
    {
        return _levelData.hasKwyRoomKey;
    }

    public void SetDashState(bool state)
    {
        _levelData.hasDash = state;
    }

    public bool GetDashStatus()
    {
        return _levelData.hasDash;
    }

    public ECamAvailable GetCamState()
    {
        return _currentSelectedCam;
    }

    public void SetNewCamState(ECamAvailable _state)
    {
        if (_state == _currentSelectedCam) return;

        _currentSelectedCam = _state;
    }

    public void AddEnemyToDeadList(GameObject enemy, Vector3 pos)
    {
        _levelData.EnemiesDead.Add(new EnemyDead(pos, enemy));
    }



    public void RespawnToCheckpoint(GameObject obj, Vector3 pos)
    {
        _levelData.TimesDied += 1;

        obj.SetActive(false);

        StartCoroutine(HandleRespawnToCheckpointAnimationPlayer(obj, pos));
    }

    private IEnumerator HandleRespawnToCheckpointAnimationPlayer(GameObject player, Vector3 position)
    {
        _deathTransition.SetActive(true);
        yield return new WaitForSeconds(_waitTransitionSeconds / 2);

        // Mudando a localização do jogador quando a tela está escura
        player.transform.position = position;
        // Reiniciando inimigos
        RespawnEnemies();
        // Mostrando a vida
        player.GetComponentInChildren<PlayerProps>().FullHeal();
        onRestartElements();

        yield return new WaitForSeconds(_waitTransitionSeconds / 2);
        _deathTransition.SetActive(false);

        player.SetActive(true);
        player.GetComponentInChildren<PlayerAnimationController>().StartRespawn();
    }

    public Vector3 LastCheckpoint() => _levelData.lastCheckpoint;
}
