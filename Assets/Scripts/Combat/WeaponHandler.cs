using System;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(Animator))]
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField] private Weapon currentWeapon;

        private Animator _animator;
        
        private static readonly int DoAttackHash = Animator.StringToHash("doAttack");

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && currentWeapon.isReady)
            {
                DoAttack();
            }
        }

        private void DoAttack()
        {
            _animator.SetTrigger(DoAttackHash);
        }

        public void SetWeaponIsAttacking()
        {
            if (currentWeapon)
            {
                currentWeapon.IsAttacking = true;
            }
        }
        public void ClearWeaponIsAttacking()
        {
            if (currentWeapon)
            {
                currentWeapon.IsAttacking = false;
            }
        }
        public void SetWeaponIsReady()
        {
            if (currentWeapon)
            {
                currentWeapon.isReady = true;
            }
        }
    }
}
