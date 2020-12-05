using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "LevelDetail", menuName = "ScriptableObjects/LevelDetailsScriptableObject", order = 1)]
    public class LevelDetailsScriptableObject : ScriptableObject
    {
        public GameObject enemyPrefab;
        public Vector3[] enemySpawns;
        public int enemiesToSpawn;
    }
}
