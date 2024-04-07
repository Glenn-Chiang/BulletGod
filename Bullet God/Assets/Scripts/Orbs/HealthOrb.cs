using UnityEngine;

public class HealthOrb : Orb
{
    [SerializeField]
    private float healAmount = 5;

    protected override void OnCollideWithPlayer()
    {

        playerStats.Heal(healAmount);
        Destroy(gameObject);        
    }
    
}