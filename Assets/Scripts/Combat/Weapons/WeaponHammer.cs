namespace Combat.Weapons
{
    public class WeaponHammer : WeaponBase
    {
        protected override void HandleSetAttack()
        {
            base.HandleSetAttack();
            PlayerCanMove = false;
            PlayerCanTurn = false;
        }

        protected override void HandleClearAttack()
        {
            base.HandleClearAttack();
            PlayerCanMove = true;
            PlayerCanTurn = true;
        }
    }
}
