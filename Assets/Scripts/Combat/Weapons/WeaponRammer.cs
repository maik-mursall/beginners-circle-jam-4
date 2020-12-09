using UnityEngine;

namespace Combat.Weapons
{
    public class WeaponRammer : WeaponBase
    {
        [SerializeField] private CharacterController playerCharacterController;
        [SerializeField] private float ramSpeed;
        
        protected override void HandleSetAttack()
        {
            base.HandleSetAttack();
            PlayerCanMove = false;
            // PlayerCanTurn = false;
        }

        protected override void HandleClearAttack()
        {
            base.HandleClearAttack();
            PlayerCanMove = true;
            // PlayerCanTurn = true;
        }

        private void FixedUpdate()
        {
            if (IsAttacking && playerCharacterController)
            {
                playerCharacterController.Move(playerCharacterController.transform.forward * (ramSpeed * Time.fixedDeltaTime));
            }
        }
    }
}
