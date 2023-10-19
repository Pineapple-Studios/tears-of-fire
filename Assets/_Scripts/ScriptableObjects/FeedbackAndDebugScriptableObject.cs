using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(fileName = "FeedbackAndDebug", menuName = "FeedbackAndDebug", order = 1)]
public class FeedbackAndDebugScriptableObject : ScriptableObject
{
    [Header("Debug Tools")]
    [Tooltip("Mostra painel de debug")]
    public bool IsDebugPanelActive = false;
    [Tooltip("Mostra mensagens do console")]
    public bool IsConsoleEnable = false;
    [Tooltip("Mostra lista dos �ltimos inputs")]
    public bool ShowLastInputList = false;
    [Tooltip("Habilita a visibilidade dos elementos de Debug")]
    public bool IsFeedbackObjectsVisible = false;
    [Tooltip("Desabilitar/Habilitar todos os inimigos")]
    public bool HasEnemies = true;
    [Tooltip("Mostrar todos os colisores")]
    public bool IsCollidersVisible = false;

    [Header("Player Engines")]
    [Tooltip("Habilita/desabilita o dash")]
    public bool IsDashEnabled = false;
    [Tooltip("Habilita/desabilita visa infinita")]
    public bool IsInifinityLife = false;

    [Header("Game play engines")]
    [Tooltip("Cenas dispon�neis")]
    public List<string> SceneList = new List<string> { };
    [Tooltip("Conclui todos os objetivos")]
    public bool IsConclude = false;
}
