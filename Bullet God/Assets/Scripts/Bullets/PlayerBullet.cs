using UnityEngine;

public class PlayerBullet : Bullet
{
    public override float Damage { get; set; }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        var damageable = collision.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.ReceiveDamage(Damage);
        }
    }
}