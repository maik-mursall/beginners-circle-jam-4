using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private SphereCollider sphereCollider;
        [SerializeField] private LayerMask layerMask;
        
        [SerializeField] private float damage = 10f;
        
        private readonly List<Damageable> _hitList = new List<Damageable>();

        private bool _isAttacking;
        public bool IsAttacking
        {
            get => _isAttacking;
            set
            {
                _isAttacking = value;

                if (!value)
                {
                    _hitList.Clear();
                }
            }
        }
        
        private void Update()
        {
            if (IsAttacking)
            {
                DoAttack();
            }
        }

        private void DoAttack()
        {
            var colliderRadius = sphereCollider.radius;
            var colliderTransform = sphereCollider.transform;
            var hits = Physics.SphereCastAll(colliderTransform.position + sphereCollider.center, colliderRadius, colliderTransform.forward, colliderRadius, layerMask);

            foreach (var raycastHit in hits)
            {
                if (raycastHit.transform.TryGetComponent(out Damageable targetDamageable))
                {
                    if (_hitList.Contains(targetDamageable)) return;
                    
                    targetDamageable.TakeDamage(damage);
                    
                    _hitList.Add(targetDamageable);
                }
            }
        }
    }
}
