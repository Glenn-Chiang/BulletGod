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

    public Rigidbody2D rb;
    public Transform firePoint;
    public EnemyBullet bulletPrefab;

    [SerializeField] ParticleSystem deathExplosion;

    public virtual float AggroDistance => 20; // Enemy will start attacking player when it is within this distance
    public virtual float MinDistance => 10; // Enemy will stop moving toward player when it is less than this distance from the player
    public virtual float AttackInterval => 2;
    public virtual float BulletDamage => 10;
    public virtual float BulletPower => 10;
    public virtual float MoveSpeed => 10;

    [SerializeField]
    protected State state;
    protected Vector2 playerPosition;
    protected float distanceFromPlayer;
    private Vector2 startPosition;
    private Vector2 roamDestination;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Start in idle roaming state
        state = State.Idle;
        startPosition = rb.position;
        roamDestination = GetRoamDestination();
    }

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager.GameState == GameManager.State.GameOver) return;
        
        gameManager.OnGameOver += HandleGameOver;

        var player = GameObject.Find("Player");
        playerControl = player.GetComponent<PlayerControl>();
        playerStats = player.GetComponent<PlayerStats>();

    }

    protected void FixedUpdate()
    {
        if (gameManager.GameState == GameManager.State.Active)
        {
            TrackPlayer();
        }

        switch (state)
        {
            default:
            case State.Idle:
                StopAttacking();

                // Roam randomly
                MoveTo(roamDestination);
                float reachedDestinationDistance = 1; // Minimum distance from destination required to consider the enemy to have reached the destination
                if (Vector2.Distance(rb.position, roamDestination) < reachedDestinationDistance)
                {
                    // Determine the next position to roam towards
                    roamDestination = GetRoamDestination();
                }

                break;

            case State.Aggro:
                // Rotate towards and aim at player
                Vector2 aimDirection = playerPosition - rb.position;
                float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
                rb.MoveRotation(angle);

                // Move towards player until it is MinDistance away from player
                if (distanceFromPlayer > MinDistance)
                {

                    MoveTo(playerPosition);
                }

                StartAttacking();
                
                break;
        }
    }

    protected virtual void StartAttacking()
    {
        // Start shooting if not already shooting
        if (!IsInvoking(nameof(Shoot)))
        {
            // Shoot at intervals
            InvokeRepeating(nameof(Shoot), 0, AttackInterval);
        }
    }

    protected virtual void StopAttacking()
    {
        // If enemy is shooting, stop shooting
        if (IsInvoking(nameof(Shoot)))
        {
            CancelInvoke(nameof(Shoot));
        }
    }

    // Constantly keep track of the player's position to determine whether to be in Idle or Aggro state
    private void TrackPlayer()
    {
        playerPosition = playerControl.rb.position;
        distanceFromPlayer = Vector2.Distance(rb.position, playerPosition);

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
        Vector2 moveDir = destination - rb.position;
        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        rb.MoveRotation(angle);
        rb.MovePosition(Vector2.MoveTowards(rb.position, destination, MoveSpeed * Time.deltaTime));
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
    public void ReceiveDamage(float damage)
    {
        HitPoints -= damage;
        if (HitPoints <= 0)
        {
            Die();
        }
    }
    private void HandleGameOver(object sender, EventArgs e)
    {
        state = State.Idle;
    }

    protected void Die()
    {
        Destroy(gameObject);

        // Spawn explosion
        Instantiate(deathExplosion, transform.position, Quaternion.identity);

        // When destroyed, award player with XP
        playerStats.ReceiveXP(XP_reward);

    }
}