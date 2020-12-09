using System;
using System.Collections.Generic;
using Gameplay.HypeMeter;
using UnityEngine;

namespace Combat.Weapons
{
    public class WeaponBase : MonoBehaviour
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

        private HypeMeter _hypeMeter;
        
        [SerializeField] private SphereCollider sphereCollider;
        
        [SerializeField] private float damage = 10f;

        [SerializeField] private KeyCode attackKeyCode;
        public KeyCode AttackKeyCode => attackKeyCode;

        [SerializeField] private String animatorAttackString;
        private int _animatorAttackHash;
        public int AnimatorAttackHash => _animatorAttackHash;

        private readonly HashSet<DamageHitInfo> _hitList = new HashSet<DamageHitInfo>();

        public bool isReady = true;

        protected bool IsAttacking;
        public bool GetIsAttacking
        {
            get => IsAttacking;
            set
            {
                IsAttacking = value;

                sphereCollider.enabled = value;

                if (!value)
                {
                    HandleClearAttack();
                }
                else
                {
                    HandleSetAttack();
                }
            }
        }

        protected bool PlayerCanMove = true;
        public bool GetPlayerCanMove => PlayerCanMove;

        protected bool PlayerCanTurn = true;
        public bool GetPlayerCanTurn => PlayerCanTurn;

        private void Awake()
        {
            _animatorAttackHash = Animator.StringToHash(animatorAttackString);
        }

        private void Start()
        {
            _hypeMeter = HypeMeter.Instance;
        }

        private void EvaluateAttack(DamageHitInfo hitInfo)
        {
            var distancePercent = hitInfo.Distance / hitInfo.Width;

            var hitSlop = 1 + sphereCollider.radius / hitInfo.Width;
            if (distancePercent <= hitSlop)
            {
                var damageFactor =  hitSlop - distancePercent;
                var wasCritical = damageFactor >= hitInfo.Damageable.CritHitDistance;
                var relevantDamageFactor = wasCritical ? 2f : damageFactor;
                
                hitInfo.Damageable.TakeDamage(damage * relevantDamageFactor, wasCritical);
                
                _hypeMeter.AddRelativeHype(relevantDamageFactor);
            }
        }

        private float CalculateDistanceToOther(Collider other)
        {
            var colliderPosition = sphereCollider.transform.position + sphereCollider.center;
            
            var flattenedColliderPosition = new Vector3
            {
                x = colliderPosition.x,
                y = 0,
                z = colliderPosition.z
            };

            var targetCenter = other.bounds.center;
            var flattenedTargetPosition = new Vector3
            {
                x = targetCenter.x,
                y = 0,
                z = targetCenter.z
            };
            
            return Vector3.Distance(flattenedTargetPosition, flattenedColliderPosition);
        }

        protected virtual void HandleSetAttack()
        {
            isReady = false;
        }

        protected virtual void HandleClearAttack()
        {
            foreach (var damageHitInfo in _hitList)
            {
                EvaluateAttack(damageHitInfo);
            }

            _hitList.Clear();
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
                        _hitList.Remove(damageHitInfo);

                        return;
                    }
                }
            }
        }
    }
}
