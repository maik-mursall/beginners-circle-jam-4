﻿using System;
using UnityEngine;

namespace Combat
{
    public class DamageEventArgs : EventArgs
    {
        public float Damage;
        public bool WasCritical;
    }
    
    public class Damageable : MonoBehaviour
    {
        public float Health { get; private set; } = 50f;
        [SerializeField] private float maxHealth = 50f;
        [SerializeField] private float critHitDistance = 0.9f;

        public float CritHitDistance => critHitDistance;

        public float GetHealthPercentage => Health / maxHealth;

        public event EventHandler Died;
        public event EventHandler Damaged;
        public event EventHandler Healed;

        private void OnEnable()
        {
            Health = maxHealth;
        }

        public void Heal(float amount)
        {
            Health = Mathf.Clamp(Health + amount, 0f, maxHealth);
            OnHeal(EventArgs.Empty);
        }

        public void TakeDamage(float damage, bool wasCritical = false)
        {
            Health -= damage;
            if (Health <= 0f)
            {
                OnDeath(EventArgs.Empty);
            }
            else
            {
                OnDamage(new DamageEventArgs
                {
                    WasCritical = wasCritical,
                    Damage = damage
                });
            }
        }

        private void OnDamage(DamageEventArgs e)
        {
            Damaged?.Invoke(this, e);
        }

        private void OnDeath(EventArgs e)
        {
            Died?.Invoke(this, e);
        }

        private void OnHeal(EventArgs e)
        {
            Healed?.Invoke(this, e);
        }
    }
}
