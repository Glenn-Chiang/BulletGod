using UnityEngine;

public class EnemySniper : Enemy
{
    [SerializeField]
    private float _health = 40;
    public override float HitPoints { get => _health; protected set => _health = value; }

    [SerializeField]
    private float xpReward = 10;
    public override float XP_reward => xpReward;

    public float attackInterval = 1;
    public override float AttackInterval => attackInterval;

    public float aggroDistance = 40;
    public override float AggroDistance => aggroDistance;

    public float minDistance = 10;
    public override float MinDistance => minDistance;

    public float bulletPower = 40;
    public override float BulletPower => bulletPower;
    public float bulletDamage = 40;
    public override float BulletDamage => bulletDamage;

    public float moveSpeed = 5;
    public override float MoveSpeed => moveSpeed;



    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        
    }

    
}