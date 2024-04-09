using UnityEngine;

public class ChargeOrb : Orb
{
    protected override void OnCollideWithPlayer()
    {
        playerStats.AddCharge();
        Destroy(gameObject);
    }

}