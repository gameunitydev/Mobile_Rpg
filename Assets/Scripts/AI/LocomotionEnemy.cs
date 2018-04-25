public class LocomotionEnemy : Locomotion
{
    public override void Attack()
    {
        base.Attack();

        if (targetLocomotion && !targetLocomotion.health.isDead)
        {
            targetLocomotion.health.Damage(gameObject.GetComponent<HealthEnemy>().enemyWeapon.GetDamage());
        }
    }
}