using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cell : MonoBehaviour, IDamageable
{
    public abstract float HitPoints { get; protected set; }

    [SerializeField]
    private GameObject orb;
    private readonly int numOrbs = 4;

    protected PlayerStats playerStats;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        var player = GameObject.Find("Player");
        playerStats = player.GetComponent<PlayerStats>();
        
    }

    private void FixedUpdate()
    {
        // Prevent cell from moving beyond WorldMap boundaries
        Vector2 velocity = rb.velocity;
        if ((velocity.x < 0 && rb.position.x <= WorldMap.minX) || (velocity.x > 0 && rb.position.x >= WorldMap.maxX))
        {
            velocity.x = 0;
        }
        if ((velocity.y < 0 && rb.position.y <= WorldMap.minY) || (velocity.y > 0 && rb.position.y >= WorldMap.maxY))
        {
            velocity.y = 0;
        }

        rb.velocity = velocity;
    }

    public void ReceiveDamage(float damage)
    {
        HitPoints -= damage;
        if (HitPoints <= 0)
        {
            OnDestroyed();
        }
    }

    protected virtual void OnDestroyed()
    {
        Destroy(gameObject);
        // Spawn orbs when destroyed
        for (int i = 0; i < numOrbs; i++)
        {
            float randomXOffset = UnityEngine.Random.Range(0f, 0.5f);
            float randomYOffset = UnityEngine.Random.Range(0f, 0.5f);
            Vector2 spawnPosition = new Vector2(transform.position.x + randomXOffset, transform.position.y + randomYOffset);
            Instantiate(orb, spawnPosition, transform.rotation);
        }
    }
}
