using UnityEngine;

public class EnemyBullet : Bullet
{
    public override float Damage { get; set; }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        // Check if the bullet collided with the player
        var playerStats = collision.collider.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.ReceiveDamage(Damage);
        }
    }
}