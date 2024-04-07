using UnityEngine;

public class EnemyBullet : Bullet
{
    public override float Damage { get; set; }

    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the bullet collided with the player
        if (collision.collider.CompareTag("Player"))
        {
            playerStats.ReceiveDamage(Damage);
        }
        Destroy(gameObject);
    }
}