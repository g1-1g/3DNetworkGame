using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float damage, int attackerActorNumber);

    public void Kill(EDieType type);
}
