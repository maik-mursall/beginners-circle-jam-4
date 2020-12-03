using System;
using Combat;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Damageable))]
    public class EnemyBase : MonoBehaviour
    {
        private Damageable _damageable;

        private void Start()
        {
            _damageable = GetComponent<Damageable>();

            _damageable.Died += OnDeath;
            _damageable.Damaged += OnDamage;
        }

        private void OnDeath(object src, EventArgs args)
        {
            Destroy(gameObject);
        }
        private void OnDamage(object src, EventArgs args)
        {
            var damageEventArgs = (DamageEventArgs) args;
            Debug.Log($"Took {damageEventArgs.Damage} {(damageEventArgs.WasCritical ? "critical" : "")} damage!");
        }
    }
}
