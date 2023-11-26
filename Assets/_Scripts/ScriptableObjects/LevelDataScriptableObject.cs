using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "DataStorage/LevelData", order = 2)]

public class LevelDataScriptableObject : ScriptableObject
{
    [System.Serializable]
    public struct EnemyDead
    {
        public GameObject GameObject;
        public Vector3 Position;

        public EnemyDead(Vector3 position, GameObject gameObject)
        {
            GameObject = gameObject;
            Position = position;
        }
    }

    public string currentLevelName;
    public Vector3 lastCheckpoint;

    [Header("Game info")]
    public int TimesDied;

    [Header("Player props")]
    public bool hasDash = false;

    [Header("Enemies")]
    public List<EnemyDead> EnemiesDead;
}
