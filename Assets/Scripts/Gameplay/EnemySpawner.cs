using System;
using Combat;
using Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner Instance;
        
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private Transform[] enemySpawns;
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
                SpawnEnemies(_currentAmount + 1);   
            }
        }

        public void SpawnEnemies(int amount)
        {
            _currentAmount = amount;
            _enemiesAlive = amount;

            var currentPlayerObject = GameManager.Instance.GetCurrentPlayerGameObject;

            for (int i = 0; i < amount; i++)
            {
                var relevantSpawn = enemySpawns[Random.Range(0, enemySpawns.Length)];
                var enemyInstance = Instantiate(enemyPrefab, relevantSpawn.position, relevantSpawn.rotation, enemiesParent);

                enemyInstance.GetComponent<Damageable>().Died += OnEnemyKilled;
                enemyInstance.GetComponent<EnemyMovement>().SetTarget(currentPlayerObject.transform);
            }
        }
    }
}
