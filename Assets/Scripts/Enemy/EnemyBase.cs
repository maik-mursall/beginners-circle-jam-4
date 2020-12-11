using System;
using Combat;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Damageable))]
    public class EnemyBase : MonoBehaviour
    {
        private Damageable _damageable;
        public event Action OnEnemyDeath;

        private void Start()
        {
            _damageable = GetComponent<Damageable>();

            _damageable.Died += OnDeath;

            if (OnEnemyDeath != null)
                OnEnemyDeath();
        }

        private void OnDeath(object src, EventArgs args)
        {
            Destroy(gameObject);
        }
    }
}
