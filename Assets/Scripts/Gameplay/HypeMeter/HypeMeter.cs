using UnityEngine;

namespace Gameplay.HypeMeter
{
    public class HypeMeter : MonoBehaviour
    {
        public static HypeMeter Instance;

        [SerializeField] private float maxHype = 100f;
        [SerializeField] private float hypeDegradationPerSecond = 1f;
        [SerializeField] private float maxHypeGain = 20f;
        private float _currentHypePercent = 50f;

        public float HypePercent => _currentHypePercent / maxHype;

        private void Awake()
        {
            if (Instance) DestroyImmediate(this);

            Instance = this;
        }

        private void Update()
        {
            _currentHypePercent -= hypeDegradationPerSecond * Time.deltaTime;

            if (_currentHypePercent <= 0f)
            {
                GameManager.Instance.GameOver();
            }
        }

        public void AddRelativeHype(float factor)
        {
            _currentHypePercent += maxHypeGain * factor;
        }
    }
}
