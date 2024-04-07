using UnityEngine;

public class EnemyTank : Enemy
{
    [SerializeField]
    private float _health = 100;
    public override float HitPoints { get => _health; protected set => _health = value; }

    [SerializeField]
    private float xpReward = 10;
    public override float XP_reward => xpReward;

    public float attackInterval = 2;
    public override float AttackInterval => attackInterval;
    public float bulletPower = 100;
    public override float BulletPower => bulletPower;
    public float bulletDamage = 40;
    public override float BulletDamage => bulletDamage;

    public float moveSpeed = 2;
    public override float MoveSpeed => moveSpeed;

    public float minDistance = 0;
    public override float MinDistance => minDistance;

  


}