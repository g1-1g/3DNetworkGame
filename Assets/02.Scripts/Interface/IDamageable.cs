using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float Damage);

    public void Kill(EDieType type);
}
