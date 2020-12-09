using System.Linq;
using Combat.Weapons;
using Gameplay;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(Animator))]
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField] private WeaponBase[] weapons;

        [SerializeField] private Animator animator;
        
        public bool PlayerCanMove => weapons.All(weapon => weapon.GetPlayerCanMove);
        public bool PlayerCanTurn => weapons.All(weapon => weapon.GetPlayerCanTurn);

        private void Update()
        {
            if (!GameManager.Instance.IsGameRunning || SpawnManager.Instance.WaveIsPreparing) return;
            if (weapons.Any(weapon => weapon.GetIsAttacking)) return;

            foreach (var weapon in weapons)
            {
                if (Input.GetKeyDown(weapon.AttackKeyCode) && !weapon.OnCooldown)
                {
                    DoAttack(weapon.AnimatorAttackHash);
                }
            }
        }

        private void DoAttack(int animationHash)
        {
            animator.SetTrigger(animationHash);
        }

        public void SetWeaponIsAttacking(AnimationEvent animationEvent)
        {
            if (weapons.ElementAtOrDefault(animationEvent.intParameter) is var weapon && weapon != null)
            {
                weapon.GetIsAttacking = true;
            }
        }

        public void ClearWeaponIsAttacking(AnimationEvent animationEvent)
        {
            if (weapons.ElementAtOrDefault(animationEvent.intParameter) is var weapon && weapon != null)
            {
                weapon.GetIsAttacking = false;
            }
        }
    }
}
