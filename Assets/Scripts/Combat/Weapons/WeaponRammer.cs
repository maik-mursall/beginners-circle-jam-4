using Player;
using UnityEngine;

namespace Combat.Weapons
{
    public class WeaponRammer : WeaponBase
    {
        [SerializeField] private CharacterController playerCharacterController;
        [SerializeField] private PlayerAim playerAim;
        [SerializeField] private float ramSpeed = 20f;
        [SerializeField] private float ramTurnSpeed = 5f;
        
        protected override void HandleSetAttack()
        {
            base.HandleSetAttack();
            PlayerCanMove = false;
            // PlayerCanTurn = false;
            
            playerAim.SetCurrentTurnSpeed(ramTurnSpeed);
        }

        protected override void HandleClearAttack()
        {
            base.HandleClearAttack();
            PlayerCanMove = true;
            // PlayerCanTurn = true;
            
            playerAim.ResetCurrentTurnSpeed();
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
