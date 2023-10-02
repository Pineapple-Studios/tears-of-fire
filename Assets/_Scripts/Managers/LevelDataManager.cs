using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataManager : MonoBehaviour
{
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
        PlayerProps.onPlayerDead += Respawn;
    }

    private void OnDisable()
    {
        Checkpoint.NewCheckpoint -= SetLastCheckpoint;
        PlayerProps.onPlayerDead -= Respawn;
    }

    private void Respawn(GameObject obj)
    {
        _levelData.TimesDied += 1;
        
        // TODO: Criar transição de morte do SVART
        
        obj.SetActive(false);
        // Reinicializando o svart
        obj.GetComponentInChildren<Animator>().Play("idle");

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

        yield return new WaitForSeconds(_waitTransitionSeconds / 2);
        _deathTransition.SetActive(false);

        player.SetActive(true);
    }



    private void SetLastCheckpoint(Vector3 trans)
    {
        _levelData.lastCheckpoint = trans;
    }
}
