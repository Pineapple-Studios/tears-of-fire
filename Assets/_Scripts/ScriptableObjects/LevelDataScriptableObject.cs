using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "DataStorage/LevelData", order = 1)]

public class LevelDataScriptableObject : ScriptableObject
{
    public string currentLevelName;
    public Vector3 lastCheckpoint;

    [Header("Game info")]
    public int TimesDied;
}
