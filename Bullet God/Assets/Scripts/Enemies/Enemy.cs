using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    protected enum State
    {
        Idle,
        Aggro,
    }

    public abstract float HitPoints { get; protected set; }

    public abstract float XP_reward { get; }

    private GameManager gameManager;
    protected PlayerStats playerStats;
    protected PlayerControl playerControl;

    public Rigidbody2D rigidBody;
    public Transform firePoint;
    public EnemyBullet bulletPrefab;

    [SerializeField] ParticleSystem explosionPrefab;

    public virtual float AggroDistance => 20; // Enemy will start attacking player when it is within this distance
    public virtual float MinDistance => 10; // Enemy will stop moving toward player when it is less than this distance from the player
    public virtual float AttackInterval => 2;
    public virtual float BulletPower => 10;
    public virtual float MoveSpeed => 10;
    public virtual float BulletDamage => 10;

    [SerializeField]
    protected State state;
    protected Vector2 playerPosition;
    protected float distanceFromPlayer;
    private Vector2 startPosition;
    private Vector2 roamDestination;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.OnGameOver += HandleGameOver;

        var player = GameObject.Find("Player");
        playerControl = player.GetComponent<PlayerControl>();
        playerStats = player.GetComponent<PlayerStats>();

        state = State.Idle;
        startPosition = rigidBody.position;
        roamDestination = GetRoamDestination();
    }

    protected virtual void FixedUpdate()
    {
        if (gameManager.GameState == GameManager.State.Active)
        {
            TrackPlayer();
        }

        switch (state)
        {
            default:
            case State.Idle:
                // If enemy is shooting, stop shooting
                if (IsInvoking(nameof(Shoot)))
                {
                    CancelInvoke(nameof(Shoot));
                }

                // Roam randomly
                MoveTo(roamDestination);
                float reachedDestinationDistance = 1; // Minimum distance from destination required to consider the enemy to have reached the destination
                if (Vector2.Distance(rigidBody.position, roamDestination) < reachedDestinationDistance)
                {
                    // Determine the next position to roam towards
                    roamDestination = GetRoamDestination();
                }

                break;

            case State.Aggro:
                // Rotate towards and aim at player
                Vector2 aimDirection = playerPosition - rigidBody.position;
                float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
                rigidBody.MoveRotation(angle);

                // Move towards player until it is MinDistance away from player
                if (distanceFromPlayer > MinDistance)
                {

                    MoveTo(playerPosition);
                }

                // Start shooting if not already shooting
                if (!IsInvoking(nameof(Shoot)))
                {
                    // Shoot at intervals
                    InvokeRepeating(nameof(Shoot), 0, AttackInterval);
                }
                
                break;
        }
    }

    private void HandleGameOver(object sender, EventArgs e)
    {
        state = State.Idle;
    }

    // Randomly decide on a position to move toward while roaming in idle state
    private Vector2 GetRoamDestination()
    {
        var randomDirection = new Vector2(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1)).normalized;
        float minRoamDistance = 5;
        float maxRoamDistance = 20;
        var randomDistance = UnityEngine.Random.Range(minRoamDistance, maxRoamDistance);
        Vector2 destination = startPosition + randomDirection * randomDistance;
        // Prevent roaming beyond WorldMap boundaries
        destination.x = Mathf.Clamp(destination.x, WorldMap.minX, WorldMap.maxX);
        destination.y = Mathf.Clamp(destination.y, WorldMap.minY, WorldMap.maxY);
        return destination;
    }

    // Constantly keep track of the player's position to determine whether to be in Idle or Aggro state
    private void TrackPlayer()
    {
        playerPosition = playerControl.rb.position;
        distanceFromPlayer = Vector2.Distance(rigidBody.position, playerPosition);

        if (distanceFromPlayer > AggroDistance)
        {
            state = State.Idle;
        }
        else
        {
            state = State.Aggro;

        }
    }

    protected void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        var bulletBody = bullet.GetComponent<Rigidbody2D>();
        var bulletForce = firePoint.right * BulletPower;
        bulletBody.AddForce(bulletForce, ForceMode2D.Impulse);
        var bulletLogic = bullet.GetComponent<EnemyBullet>();
        bulletLogic.Damage = BulletDamage;
    }
    private void MoveTo(Vector2 destination)
    {
        rigidBody.MovePosition(Vector2.MoveTowards(rigidBody.position, destination, MoveSpeed * Time.deltaTime));
    }
    public void ReceiveDamage(float damage)
    {
        HitPoints -= damage;
        if (HitPoints <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        Destroy(gameObject);

        // Spawn explosion
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // When destroyed, award player with XP
        playerStats.ReceiveXP(XP_reward);

    }
}