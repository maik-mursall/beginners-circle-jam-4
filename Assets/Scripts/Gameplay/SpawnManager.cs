using System;
using System.Linq;
using Combat;
using Enemy;
using UnityEngine;

namespace Gameplay
{
    public class WaveClearedEvent : EventArgs
    {
        public int ClearedWave;
    }

    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager Instance;

        [SerializeField] private LevelDetailsScriptableObject levelDetailsScriptableObject;
        [SerializeField] private Transform enemiesParent;

        private int _enemiesAlive;
        private int _currentWave;
        
        public event EventHandler WaveComplete;
        public event EventHandler AllWavesComplete;

        private void Awake()
        {
            if (Instance) DestroyImmediate(gameObject);

            Instance = this;
        }
        
        private void OnEnemyKilled(object src, EventArgs args)
        {
            if (--_enemiesAlive <= 0)
            {
                HandleNextWaveSpawn();
            }
        }

        private void HandleNextWaveSpawn()
        {
            if (_currentWave >= levelDetailsScriptableObject.waves.Length)
            {
                AllWavesComplete?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                WaveComplete?.Invoke(this, new WaveClearedEvent{ ClearedWave = _currentWave });
                SpawnWave();
            }
        }

        public void SpawnWave()
        {
            var wave = levelDetailsScriptableObject.waves[_currentWave++];
            _enemiesAlive = wave.enemiesToSpawn.Select(enemies => enemies.amountToSpawn).Sum();

            var currentPlayerObject = GameManager.Instance.GetCurrentPlayerGameObject;

            foreach (var waveEnemyDescriptor in wave.enemiesToSpawn)
            {
                for (int i = 0; i < waveEnemyDescriptor.amountToSpawn; i++)
                {
                    var relevantSpawn =
                        waveEnemyDescriptor.positionsAndRotations[i % waveEnemyDescriptor.positionsAndRotations.Length];

                    var enemyInstance = Instantiate(waveEnemyDescriptor.prefab, relevantSpawn.position, relevantSpawn.rotation, enemiesParent);
                    enemyInstance.GetComponent<Damageable>().Died += OnEnemyKilled;
                    enemyInstance.GetComponent<EnemyMovement>().SetTarget(currentPlayerObject.transform);
                }
            }
        }
    }
}
