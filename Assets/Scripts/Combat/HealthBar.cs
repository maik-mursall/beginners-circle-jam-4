using System;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Damageable damageable;

        private void Start()
        {
            damageable.Damaged += (object src, EventArgs args) => OnDamage(src, (DamageEventArgs)args);
            slider.value = damageable.GetHealthPercentage;
        }
        
        private void OnDamage(object src, DamageEventArgs args)
        {
            slider.value = damageable.GetHealthPercentage;
        }
    }
}
