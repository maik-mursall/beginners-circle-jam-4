using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.HypeMeter;
using UnityEngine;
using UnityEngine.AI;

namespace Combat.Weapons
{
    public class CooldownUpdateEventArgs : EventArgs
    {
        public float TimeLeftPercent;
    }
    
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

        [SerializeField] private string animatorAttackString;
        public int AnimatorAttackHash { get; private set; }

        private readonly HashSet<DamageHitInfo> _hitList = new HashSet<DamageHitInfo>();

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

        [SerializeField] private float cooldown = 5f;
        private float _currentCooldown;

        private bool _cooldownActive;
        public bool OnCooldown { get; private set; }
        
        public event EventHandler CooldownStarted;
        public event EventHandler CooldownUpdate;
        public event EventHandler CooldownFinished;

        private void Awake()
        {
            AnimatorAttackHash = Animator.StringToHash(animatorAttackString);
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
            StartCoroutine(StartCooldown());
        }

        protected virtual void HandleClearAttack()
        {
            foreach (var damageHitInfo in _hitList)
            {
                EvaluateAttack(damageHitInfo);
            }

            _hitList.Clear();

            _cooldownActive = true;
        }

        private IEnumerator StartCooldown()
        {
            _currentCooldown = cooldown;
            _cooldownActive = false;
            OnCooldown = true;
            
            CooldownStarted?.Invoke(this, EventArgs.Empty);

            while(_currentCooldown > 0f)
            {
                yield return new WaitForEndOfFrame();

                if (_cooldownActive)
                {
                    _currentCooldown -= Time.deltaTime;
                    CooldownUpdate?.Invoke(this, new CooldownUpdateEventArgs{ TimeLeftPercent = (1 - (_currentCooldown / cooldown)) });
                }
            }
            
            OnCooldown = false;
            CooldownFinished?.Invoke(this, EventArgs.Empty);
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
