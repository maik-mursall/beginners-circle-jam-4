﻿using System;
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

                sphereCollider.enabled = value;

                if (!value)
                {
                    _hitList.Clear();
                }
            }
        }

        private void EvaluateAttack(DamageHitInfo hitInfo)
        {
            var distancePercent = hitInfo.Distance / hitInfo.Width;

            if (distancePercent > 0)
            {
                var damageFactor =  Mathf.Clamp01(1 - distancePercent);
                var wasCritical = damageFactor >= hitInfo.Damageable.CritHitDistance;
                hitInfo.Damageable.TakeDamage(damage * (wasCritical ? 2f : damageFactor), wasCritical);   
            }

            _hitList.Remove(hitInfo);
        }

        private float CalculateDistanceToOther(Collider other)
        {
            var colliderTransform = sphereCollider.transform;
            var colliderPosition = colliderTransform.position + sphereCollider.center;
            var flattenedColliderPosition = new Vector3
            {
                x = colliderPosition.x,
                y = 0,
                z = colliderPosition.z
            };

            var targetBounds = other.bounds;
            var targetCenter = targetBounds.center;
            var flattenedTargetPosition = new Vector3
            {
                x = targetCenter.x,
                y = 0,
                z = targetCenter.z
            };
            
            return Vector3.Distance(flattenedTargetPosition, flattenedColliderPosition);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent(out Damageable targetDamageable))
            {
                _hitList.Add(new DamageHitInfo
                    {
                        Damageable = targetDamageable,
                        Distance = CalculateDistanceToOther(other),
                        Width = other.bounds.extents.x
                    }
                );
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.transform.TryGetComponent(out Damageable targetDamageable))
            {
                foreach (var damageHitInfo in _hitList)
                {
                    var hitInfo = damageHitInfo;
                    if (hitInfo.Damageable.Equals(targetDamageable))
                    {
                        hitInfo.Distance = Mathf.Min(hitInfo.Distance, CalculateDistanceToOther(other));
                    }
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.transform.TryGetComponent(out Damageable targetDamageable))
            {
                foreach (var damageHitInfo in _hitList)
                {
                    if (damageHitInfo.Damageable.Equals(targetDamageable))
                    {
                        EvaluateAttack(damageHitInfo);

                        return;
                    }
                }
            }
        }
    }
}
