using Gameplay;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(Animator))]
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField] private Weapon currentWeapon;

        [SerializeField] private Animator animator;
        
        private static readonly int DoAttackHash = Animator.StringToHash("doAttack");

        private bool _playerCanMove = true;

        public bool PlayerCanMove => _playerCanMove;

        private void Update()
        {
            if (GameManager.Instance.IsGameRunning && Input.GetMouseButtonDown(0) && currentWeapon.isReady)
            {
                DoAttack();
            }
        }

        private void DoAttack()
        {
            animator.SetTrigger(DoAttackHash);
        }

        public void SetPlayerCanMove() => _playerCanMove = true;

        public void SetWeaponIsAttacking()
        {
            if (currentWeapon)
            {
                currentWeapon.IsAttacking = true;
                _playerCanMove = false;
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
