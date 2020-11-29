using Combat;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Damageable damageable;

        private void Update()
        {
            slider.value = Mathf.Clamp01(damageable.GetHealthPercentage);
        }
    }
}