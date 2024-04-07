using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Vector2 cursorPos;
    private Vector2 velocity;

    private PlayerStats playerStats;

    public Rigidbody2D rigidBody;
    public Transform firePoint;
    public PlayerBullet bulletPrefab;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        velocity = new Vector2(moveX, moveY).normalized;

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dash();
        }
    }

    void FixedUpdate()
    {
        // If player tries to move left beyond left edge or move right beyond right edge, stop their horizontal movement
        if ((transform.position.x <= WorldMap.minX && velocity.x < 0) || (transform.position.x >= WorldMap.maxX && velocity.x > 0))
        {
            velocity.x = 0;
        }
        // If player tries to move up beyond top edge or move down beyond bottom edge, stop their vertical movement
        if ((transform.position.y <= WorldMap.minY && velocity.y < 0) || (transform.position.y >= WorldMap.maxY && velocity.y > 0))
        {
            velocity.y = 0;
        }

        rigidBody.MovePosition(rigidBody.position + velocity * playerStats.moveSpeed * Time.deltaTime);

        // Rotate toward cursor position
        Vector2 aimDirection = cursorPos - rigidBody.position;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        rigidBody.MoveRotation(angle);
    }

    private void Dash()
    {
        Debug.Log("Dash");
        rigidBody.MovePosition(rigidBody.position + velocity * playerStats.dashSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
        var bulletForce = firePoint.right * playerStats.bulletPower;
        bullet.GetComponent<Rigidbody2D>().AddForce(bulletForce, ForceMode2D.Impulse);
        bullet.Damage = playerStats.bulletDamage;
    }
}
