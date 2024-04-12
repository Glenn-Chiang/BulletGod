using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector2 fireDirection;
    [SerializeField] private float fireDuration;
    [SerializeField] private float damage = 100f;
    private float beamWidth = 1f;

    private void Awake()
    {
        lineRenderer = gameObject.GetComponentInChildren<LineRenderer>();
        lineRenderer.startWidth = beamWidth;
        lineRenderer.endWidth = beamWidth;
        fireDirection = transform.right;
    }

    private void Start()
    {
        fireDuration = GameObject.Find("Player").GetComponent<PlayerControl>().laserDuration;
        StartCoroutine(Fire());
    }

    private IEnumerator Fire()
    {
        RenderLine();

        int layerMask = ~LayerMask.GetMask("Player"); // Include all layers except Player
        var raycastHits = Physics2D.CircleCastAll(transform.position, beamWidth, fireDirection, Mathf.Infinity, layerMask);
        //var raycastHits = Physics2D.RaycastAll(transform.position, fireDirection);
        foreach(var hit in raycastHits)
        {
            if (hit.collider.gameObject.TryGetComponent<IDamageable>(out var damageableEntity))
            {
                damageableEntity.ReceiveDamage(damage);
            }
        }

        // Fires for a short duration before disappearing
        yield return new WaitForSeconds(fireDuration);
        Destroy(gameObject);
    }

    private void RenderLine()
    {
        Vector2 endPoint = FindIntersectionWithMap();
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPoint);
    }

    private Vector2 FindIntersectionWithMap()
    {
        Vector2 startPoint = (Vector2)transform.position;
        // Find point where laser intersects with one of the map boundaries
        // The laser should only intersect with 1 one of the map boundaries
        // However, the lines of the laser and the map boundaries are treated as infinite by the FindIntersection formula,
        // which means that points of intersection will be found for all 4 map boundaries
        // We thus need to find which is the correct intersection point, i.e. the one that is within the map and in the correct direction from the aimDir
        
        foreach (var boundary in WorldMap.boundaries)
        {
            Vector2 intersectionPoint = Geometry.FindIntersection(startPoint, startPoint + fireDirection, boundary.start, boundary.end);
            Vector2 lineOfFire = intersectionPoint - startPoint;
            // Check if the point is in the direction that the laser is aiming towards
            bool sameDirection = fireDirection.x * lineOfFire.x > 0 && fireDirection.y * lineOfFire.y > 0;
            bool withinMap = WorldMap.WithinMap(intersectionPoint);
      
            if (withinMap && sameDirection)
            {
                return intersectionPoint;
            }
        }
        // This shouldn't be possible
        throw new System.Exception("No point of intersection");
    }

   
}