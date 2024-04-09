using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable, Inspectable]
public class Stat
{
    private readonly float baseValue;
    public float value;
    private readonly float maxValue;
    private readonly float factor; // Factor by which current value is multiplied whenever stat is incremented

    public Stat(float baseValue, float maxValue, int maxIncrements) // maxIncrements refers to how many times the stat can be incremented
    {
        this.baseValue = baseValue;
        value = baseValue;
        this.maxValue = maxValue;

        this.factor = CalculateFactor(maxIncrements);
    }

    // Calculate an appropriate increment factor for each increment such that maxValue will not be exceeded after maxIncrements is reached
    private float CalculateFactor(int maxIncrements)
    {
        return (float)Math.Pow(10, Math.Log10(maxValue / baseValue) / maxIncrements);
    }

    public void Increment()
    {
        value = Math.Min(value * factor, maxValue);
    }

}

public class PlayerStats: MonoBehaviour, IDamageable
{
    private float _xp = 0;
    public float XP => _xp;

    public float xpPerLevel = 100; // How much xp required to level up
    private int _level = 0;
    private const int maxLevel = 10;
    public int Level => _level;

    public Stat moveSpeed = new(10, 14, maxLevel);
    public Stat bulletDamage = new(10, 25, maxLevel);
    public Stat bulletPower = new(20, 40, maxLevel);
    public Stat maxHealth = new(100, 200, maxLevel);
    
    private float _health;
    public float Health => _health;
    public float HitPoints => _health;

    public readonly int maxCharges = 10;
    [SerializeField] private int chargeCount = 0; // Number of laser charges the player currently has
    public int ChargeCount => chargeCount;

    [SerializeField]
    private PlayerBullet playerBullet;

    private GameManager gameManager;
    public event EventHandler OnLevelUp;

    private void Awake()
    {
        _health = maxHealth.value;
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
        _health += Math.Min(health, maxHealth.value - _health);
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
        maxHealth.Increment();
        _health = maxHealth.value; // Heal to full on level up
        moveSpeed.Increment();
        bulletDamage.Increment();
        bulletPower.Increment();

        OnLevelUp?.Invoke(this, EventArgs.Empty);
    }

    public void AddCharge()
    {
        chargeCount = Math.Min(maxCharges, chargeCount + 1);
    }

    public void ConsumeCharge()
    {
        chargeCount = Math.Max(0, chargeCount - 1);
    }

    private void Die()
    {
        gameObject.SetActive(false);
        gameManager.SetGameOver();
    }

}