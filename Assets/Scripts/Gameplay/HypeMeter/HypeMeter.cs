using System;
using UnityEngine;

namespace Gameplay.HypeMeter
{
    public class HypeMeter : MonoBehaviour
    {
        public static HypeMeter Instance;

        [SerializeField] private float maxHype = 100f;
        [SerializeField] private float hypeDegradationPerSecond = 1f;
        [SerializeField] private float maxHypeGain = 20f;
        [SerializeField] private float currentHypePercent = 50f;

        public float HypePercent => currentHypePercent / maxHype;
        
        public event EventHandler HypeChanged;

        private GameManager _gameManager;

        private void Awake()
        {
            if (Instance) DestroyImmediate(this);

            Instance = this;
        }

        private void Start()
        {
            _gameManager = GameManager.Instance;
        }

        private void Update()
        {
            if (!_gameManager.IsGameRunning || SpawnManager.Instance.WaveIsPreparing) return;

            SetCurrentHypePercent(currentHypePercent - hypeDegradationPerSecond * Time.deltaTime);

            if (currentHypePercent <= 0f)
            {
                _gameManager.GameOver();
            }
        }

        public void AddRelativeHype(float factor)
        {
            SetCurrentHypePercent(currentHypePercent + maxHypeGain * factor);
        }

        public void AddAbsoluteHype(float amount)
        {
            SetCurrentHypePercent(currentHypePercent + amount);
        }

        private void SetCurrentHypePercent(float value)
        {
            currentHypePercent = Mathf.Clamp(value, 0f, maxHype);
            
            HypeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
