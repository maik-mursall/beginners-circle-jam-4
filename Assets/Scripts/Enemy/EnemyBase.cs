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
        }

        private void OnDeath(object src, EventArgs args)
        {
            Destroy(gameObject);
        }
    }
}
