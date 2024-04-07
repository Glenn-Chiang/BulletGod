using UnityEngine;

public interface IDamageable
{
    public abstract float HitPoints { get; }

    public void ReceiveDamage(float damage);
}