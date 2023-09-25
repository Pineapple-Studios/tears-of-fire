using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataManager : MonoBehaviour
{
    [SerializeField]
    private LevelDataScriptableObject _levelData;

    [Header("Respawn Elements")]
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private CameraFollowObject _cameraFollowObject;
    [SerializeField]
    private CinemachineVirtualCamera _virtualCamera;
    [SerializeField]
    private ScenarioColorManager _colorManager;

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

    private void Respawn()
    {
        _levelData.TimesDied += 1;
        // TODO: Criar transição de morte aqui
        GameObject obj = Instantiate(_player, _levelData.lastCheckpoint, Quaternion.identity);
        CurrentPlayerObject = obj;
        AddReferenceToRequiredElements(obj);
    }

    private void AddReferenceToRequiredElements(GameObject obj)
    {
        // Define a referencia pro gerenciador de cores/luz do cenário
        _colorManager.SetFollowTo(obj.transform);
        // Define a referencia do elemento que a camera está definindo como meio da tela
        _cameraFollowObject.SetFollowTo(obj.transform);
        // Definindo a referência na câmera virtual
        _virtualCamera.Follow = obj.transform;
    }

    private void SetLastCheckpoint(Vector3 trans)
    {
        _levelData.lastCheckpoint = trans;
    }
}
