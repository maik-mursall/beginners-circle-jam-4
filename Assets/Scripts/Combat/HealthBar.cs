using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Slider ghostSlider;
        [SerializeField] private Damageable damageable;

        private float _ghostPercentage = 0f;

        private void Start()
        {
            damageable.Damaged += (src, args) => OnDamage(src, (DamageEventArgs)args);
            slider.value = damageable.GetHealthPercentage;

            _ghostPercentage = slider.value;
            ghostSlider.value = _ghostPercentage;
        }

        private void OnDamage(object src, DamageEventArgs args)
        {
            slider.DOValue(damageable.GetHealthPercentage, 1f);
            ghostSlider.DOValue(damageable.GetHealthPercentage, 1f).SetDelay(1f);
        }
    }
}
