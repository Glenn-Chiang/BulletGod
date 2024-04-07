using System;
using UnityEngine;

public class PlayerStats: MonoBehaviour, IDamageable
{
    public float moveSpeed = 10;
    private float maxMoveSpeed = 20;

    public float dashSpeed = 40;

    public float bulletDamage = 10;
    public float maxBulletDamage = 40;

    public float bulletPower = 20; // Affects speed of bullet
    private float maxBulletPower = 40;

    private float _health;
    public float maxHealth = 100;
    private float maxMaxHealth = 500; // maxHealth cannot be higher than this

    public float Health => _health;
    public float HitPoints => _health;

    private float _xp = 0;
    public float XP => _xp;

    public float xpPerLevel = 100; // How much xp required to level up
    private int _level = 0;
    public int Level => _level;

    private PlayerControl playerControl;
    [SerializeField]
    private PlayerBullet playerBullet;

    private GameManager gameManager;

    private void Awake()
    {
        _health = maxHealth;    
        playerControl = GetComponent<PlayerControl>();
    }

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void ReceiveDamage(float damage)
    {
        if (damage < _health)
        {
            _health -= damage;
        } else
        {
            _health = 0;
            Die();
        }
    }

    public void Heal(float health)
    {
        _health += Math.Min(health, maxHealth - _health);
    }

    public void ReceiveXP(float xp)
    {
        if (_xp + xp < xpPerLevel)
        {
            _xp += xp;
        } else
        {
            _xp = _xp + xp - xpPerLevel;
            LevelUp();
        }
        
    }

    private void LevelUp()
    {
        _level ++;
        maxHealth = IncreaseStat(maxHealth, maxMaxHealth);
        _health = maxHealth;
        moveSpeed = IncreaseStat(moveSpeed, maxMoveSpeed);
        bulletDamage = IncreaseStat(bulletDamage, maxBulletDamage);
        bulletPower = IncreaseStat(bulletPower, maxBulletPower);
    }

    private float IncreaseStat(float currentStat,  float maxStat, float multiplier = 1.1f)
    {
        return Math.Min(currentStat * multiplier, maxStat);
    }

    private void Die()
    {
        gameObject.SetActive(false);
        gameManager.SetGameOver();
    }

}