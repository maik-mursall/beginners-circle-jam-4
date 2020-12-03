using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class Weapon : MonoBehaviour
    {
        private struct DamageHitInfo
        {
            public Damageable Damageable;
            public float Distance;
            public float Width;
            
            public override bool Equals(object other)
                => other is DamageHitInfo info && info.Damageable.Equals(Damageable);

            public override int GetHashCode() => Damageable.GetHashCode();
        }
        
        [SerializeField] private SphereCollider sphereCollider;
        [SerializeField] private LayerMask layerMask;
        
        [SerializeField] private float damage = 10f;
        
        private readonly HashSet<DamageHitInfo> _hitList = new HashSet<DamageHitInfo>();

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
            foreach (var hitInfo in _hitList)
            {
                var damageFactor =  Mathf.Clamp01(1 - (hitInfo.Distance / hitInfo.Width));
                var wasCritical = damageFactor >= hitInfo.Damageable.CritHitDistance;
                hitInfo.Damageable.TakeDamage(damage * (wasCritical ? 2f : damageFactor), wasCritical);
            }
        }

        private void DoAttack()
        {
            var colliderRadius = sphereCollider.radius;
            var colliderTransform = sphereCollider.transform;
            var colliderPosition = colliderTransform.position + sphereCollider.center;
            var hits = Physics.SphereCastAll(colliderPosition, colliderRadius, colliderTransform.forward, colliderRadius, layerMask);
            
            var flattenedColliderPosition = new Vector3
            {
                x = colliderPosition.x,
                y = 0,
                z = colliderPosition.z
            };

            foreach (var castHit in hits)
            {
                if (castHit.transform.TryGetComponent(out Damageable targetDamageable))
                {
                    var targetBounds = castHit.collider.bounds;
                    var targetCenter = targetBounds.center;
                    var flattenedTargetPosition = new Vector3
                    {
                        x = targetCenter.x,
                        y = 0,
                        z = targetCenter.z
                    };

                    var distance = Vector3.Distance(flattenedTargetPosition, flattenedColliderPosition);
                    
                    if (!_hitList.Add(new DamageHitInfo
                        {
                            Damageable = targetDamageable,
                            Distance = distance,
                            Width = targetBounds.extents.x
                        }
                    ))
                    {
                        foreach (var damageHitInfo in _hitList)
                        {
                            var hitInfo = damageHitInfo;
                            if (hitInfo.Damageable.Equals(targetDamageable))
                            {
                                hitInfo.Distance = Mathf.Min(hitInfo.Distance, distance);
                            }
                        }
                    }
                }
            }
        }
    }
}
