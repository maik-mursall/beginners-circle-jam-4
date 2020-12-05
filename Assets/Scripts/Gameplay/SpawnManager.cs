using System;
using Combat;
using Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager Instance;

        [SerializeField] private LevelDetailsScriptableObject levelDetailsScriptableObject;
        [SerializeField] private Transform enemiesParent;

        private int _currentAmount;
        private int _enemiesAlive;

        private void Awake()
        {
            if (Instance) DestroyImmediate(gameObject);

            Instance = this;
        }
        
        private void OnEnemyKilled(object src, EventArgs args)
        {
            if (--_enemiesAlive <= 0)
            {
                SpawnWave();
            }
        }

        public void SpawnWave()
        {
            _currentAmount = levelDetailsScriptableObject.enemiesToSpawn;
            _enemiesAlive = levelDetailsScriptableObject.enemiesToSpawn;

            var currentPlayerObject = GameManager.Instance.GetCurrentPlayerGameObject;

            for (int i = 0; i < levelDetailsScriptableObject.enemiesToSpawn; i++)
            {
                var relevantSpawn = levelDetailsScriptableObject.enemySpawns[Random.Range(0, levelDetailsScriptableObject.enemySpawns.Length)];
                var enemyInstance = Instantiate(levelDetailsScriptableObject.enemyPrefab, relevantSpawn, Quaternion.identity, enemiesParent);

                enemyInstance.GetComponent<Damageable>().Died += OnEnemyKilled;
                enemyInstance.GetComponent<EnemyMovement>().SetTarget(currentPlayerObject.transform);
            }
        }
    }
}
