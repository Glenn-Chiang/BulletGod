using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Vector2 cursorPos;
    private Vector2 moveDir;
    private Vector2 aimDir;

    private PlayerStats playerStats;

    public Rigidbody2D rb;
    [SerializeField] private Transform firePoint;
    [SerializeField] private PlayerBullet bulletPrefab;
    [SerializeField] private Laser laser;

    [SerializeField] private float dashSpeed = 40;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1;
    private bool isDashing = false;
    private bool canDash = true;

    [SerializeField] public float laserDuration = 0.5f;
    private bool isFiringLaser = false;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (isFiringLaser) return;

        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDir = new Vector2(moveX, moveY).normalized;

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        if (Input.GetButtonDown("Fire2") && !isFiringLaser && playerStats.ChargeCount > 0)
        {
            StartCoroutine(FireLaser());
        }

        if (canDash && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (isFiringLaser)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        
        if (!isDashing)
        {
            // If player tries to move left beyond left edge or move right beyond right edge, stop their horizontal movement
            if ((transform.position.x <= WorldMap.minX && moveDir.x < 0) || (transform.position.x >= WorldMap.maxX && moveDir.x > 0))
            {
                moveDir.x = 0;
            }
            // If player tries to move up beyond top edge or move down beyond bottom edge, stop their vertical movement
            if ((transform.position.y <= WorldMap.minY && moveDir.y < 0) || (transform.position.y >= WorldMap.maxY && moveDir.y > 0))
            {
                moveDir.y = 0;
            }

            rb.MovePosition(rb.position + moveDir * playerStats.moveSpeed.value * Time.deltaTime);
        }

        // Rotate toward cursor position
        aimDir = (cursorPos - rb.position).normalized;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        rb.MoveRotation(angle);
    }

    private IEnumerator FireLaser()
    {
        isFiringLaser = true;
        Instantiate(laser, firePoint.position, firePoint.rotation);
        playerStats.ConsumeCharge();
        rb.freezeRotation = true; // cannot rotate while laser is firing

        yield return new WaitForSeconds(laserDuration);
        isFiringLaser = false;
        rb.freezeRotation = false;
    }

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        var bulletForce = firePoint.right * playerStats.bulletPower.value;
        bullet.GetComponent<Rigidbody2D>().AddForce(bulletForce, ForceMode2D.Impulse);
        bullet.Damage = playerStats.bulletDamage.value;
    }
    private IEnumerator Dash()
    {
        canDash = false; // cannot dash while dashing or during cooldown
        isDashing = true;
        rb.velocity = moveDir * dashSpeed;
        yield return new WaitForSeconds(dashDuration); // dash happens over several frames
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown); // start cooldown
        canDash = true; // end cooldown
    }
}
