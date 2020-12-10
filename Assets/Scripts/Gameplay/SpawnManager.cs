using System;
using System.Collections.Generic;
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
        [SerializeField] private Transform enemiesInitialSpawn;
        [SerializeField] private Transform enemiesParent;

        private int _enemiesAlive;
        private int _enemiesRemainingToGoIntoPosition;
        private int _currentWave;
        
        private List<EnemyMovement> _enemyMovementInstances = new List<EnemyMovement>();

        private bool _waveIsPreparing;
        public bool WaveIsPreparing => _waveIsPreparing;

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

        private void StartBattle()
        {
            _waveIsPreparing = false;
            var currentPlayerTransform = GameManager.Instance.GetCurrentPlayerGameObject.transform;
            _enemyMovementInstances.ForEach((movement => movement.SetTarget(currentPlayerTransform)));
        }

        private void OnEnemyReachedStartPosition(object sender, EventArgs args)
        {
            ((EnemyMovement) sender).TargetReached -= OnEnemyReachedStartPosition;

            if (--_enemiesRemainingToGoIntoPosition <= 0)
            {
                StartBattle();
            }
        }

        public void SpawnWave()
        {
            _waveIsPreparing = true;
            _enemyMovementInstances.Clear();
            var wave = levelDetailsScriptableObject.waves[_currentWave++];
            _enemiesAlive = wave.enemiesToSpawn.Select(enemies => enemies.amountToSpawn).Sum();
            _enemiesRemainingToGoIntoPosition = _enemiesAlive;
            
            foreach (var waveEnemyDescriptor in wave.enemiesToSpawn)
            {
                for (int i = 0; i < waveEnemyDescriptor.amountToSpawn; i++)
                {
                    var relevantSpawn =
                        waveEnemyDescriptor.positionsAndRotations[i % waveEnemyDescriptor.positionsAndRotations.Length];

                    var enemyInstance = Instantiate(waveEnemyDescriptor.prefab, enemiesInitialSpawn.position, enemiesInitialSpawn.rotation, enemiesParent);

                    enemyInstance.GetComponent<Damageable>().Died += OnEnemyKilled;
                    
                    var enemyMovement = enemyInstance.GetComponent<EnemyMovement>();
                    _enemyMovementInstances.Add(enemyMovement);
                    enemyMovement.TargetReached += OnEnemyReachedStartPosition;
                    enemyMovement.SetStaticPosition(relevantSpawn.position);
                }
            }
        }
    }
}
