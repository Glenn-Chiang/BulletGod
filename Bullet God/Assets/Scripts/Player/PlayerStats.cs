using System;
using UnityEngine;

public class PlayerStats: MonoBehaviour, IDamageable
{
    public class Stat
    {

    }

    public const float baseSpeed = 10;
    public float moveSpeed = baseSpeed;
    private float maxMoveSpeed = 14;

    public const float baseBulletDamage = 10;
    public float bulletDamage = baseBulletDamage;
    private float maxBulletDamage = 25;

    public const float baseBulletPower = 20; // Affects speed of bullet
    public float bulletPower = baseBulletPower;
    private float maxBulletPower = 40;

    public const float baseMaxHealth = 100;
    public float maxHealth = baseMaxHealth;
    private float _health = baseMaxHealth;
    private const float maxMaxHealth = 500; 

    public float Health => _health;
    public float HitPoints => _health;

    private float _xp = 0;
    public float XP => _xp;

    public float xpPerLevel = 100; // How much xp required to level up
    private int _level = 0;
    private int maxLevel = 10;
    public int Level => _level;

    private PlayerControl playerControl;
    [SerializeField]
    private PlayerBullet playerBullet;

    private GameManager gameManager;

    public event EventHandler OnLevelUp;

    private void Awake()
    {   
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
        _health = maxHealth; // Heal to full on level up
        moveSpeed = IncreaseStat(moveSpeed, maxMoveSpeed);
        bulletDamage = IncreaseStat(bulletDamage, maxBulletDamage);
        bulletPower = IncreaseStat(bulletPower, maxBulletPower);

        OnLevelUp?.Invoke(this, EventArgs.Empty);
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