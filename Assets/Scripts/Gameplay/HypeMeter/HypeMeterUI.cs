using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.HypeMeter
{
    public class HypeMeterUI : MonoBehaviour
    {
        private HypeMeter _hypeMeter;
        [SerializeField] private Slider slider;

        void Start()
        {
            _hypeMeter = HypeMeter.Instance;
            slider.value = _hypeMeter.HypePercent;
        }

        void Update()
        {
            slider.value = _hypeMeter.HypePercent;
        }
    }
}
