using UnityEngine;

public class EnemyBomber : Enemy
{
    [SerializeField] private float _health = 40;
    public override float HitPoints { get => _health; protected set => _health = value; }

    [SerializeField] private float xpReward = 10;
    public override float XP_reward => xpReward;

    public float moveSpeed = 20;
    public override float MoveSpeed => moveSpeed;

    public float minDistance = 4f;
    public override float MinDistance => minDistance;

    public override float BulletDamage => 0;
    public override float BulletPower => 0;

    [SerializeField] private float explosionDamage = 90;
    [SerializeField] ParticleSystem explosionPrefab;

    protected override void StartAttacking(){}

    protected override void StopAttacking(){}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Die();
        playerStats.ReceiveDamage(explosionDamage);
    }
}