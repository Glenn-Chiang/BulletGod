using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public abstract float Damage { get; set; }

    public void Update()
    {
       // Destroy bullet if it goes beyond WorldMap boundaries
       if (transform.position.x < WorldMap.minX || transform.position.x > WorldMap.maxX 
            || transform.position.y < WorldMap.minY || transform.position.y > WorldMap.maxY)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        
    }
}
