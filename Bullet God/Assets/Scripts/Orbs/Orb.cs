using System;
using UnityEngine;

public abstract class Orb : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10;
    protected virtual float MoveSpeed => moveSpeed;
    
    public float magnetDistance = 10; 
    protected virtual float MagnetDistance => magnetDistance; 

    private Rigidbody2D rb;

    protected PlayerControl playerControl;
    protected PlayerStats playerStats;
    private GameManager gameManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        if (gameManager.GameState == GameManager.State.GameOver) return;

        var player = GameObject.Find("Player");
        playerControl = player.GetComponent<PlayerControl>();
        playerStats = player.GetComponent<PlayerStats>();
    }

    private void FixedUpdate()
    {
        if (gameManager.GameState == GameManager.State.GameOver) return;

        // Move towards player if near enough
        float distanceFromPlayer = Vector2.Distance(playerControl.transform.position, rb.transform.position);
        if (distanceFromPlayer <= MagnetDistance)
        {
            rb.MovePosition(Vector2.MoveTowards(transform.position, playerControl.transform.position, MoveSpeed * Time.deltaTime));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            OnCollideWithPlayer();
        }
    }

    protected abstract void OnCollideWithPlayer();

    
}