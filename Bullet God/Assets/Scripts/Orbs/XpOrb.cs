using UnityEngine;

public class XPOrb : Orb
{
    [SerializeField]
    private float xp = 5;

    protected override void OnCollideWithPlayer()
    {
        playerStats.ReceiveXP(xp);
        Destroy(gameObject);
    }

}