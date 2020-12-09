namespace Combat.Weapons
{
    public class WeaponSpinner : WeaponBase
    {
        protected override void HandleSetAttack()
        {
            base.HandleSetAttack();
            PlayerCanTurn = false;
        }

        protected override void HandleClearAttack()
        {
            base.HandleClearAttack();
            PlayerCanTurn = true;
        }
    }
}
