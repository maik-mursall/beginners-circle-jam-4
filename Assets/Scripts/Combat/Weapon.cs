using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private SphereCollider sphereCollider;
        [SerializeField] private LayerMask layerMask;
        
        [SerializeField] private float damage = 10f;
        
        private readonly HashSet<Damageable> _hitList = new HashSet<Damageable>();

        private bool _isAttacking;
        public bool IsAttacking
        {
            get => _isAttacking;
            set
            {
                _isAttacking = value;

                if (!value)
                {
                    EvaluateAttack();
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

        private void EvaluateAttack()
        {
            foreach (var damageable in _hitList)
            {
                damageable.TakeDamage(damage);
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
                   _hitList.Add(targetDamageable);
                }
            }
        }
    }
}
