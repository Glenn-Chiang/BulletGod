using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Vector3 offset;
    [SerializeField] private GameObject entity; // The entity that the healthbar is attached to
    [SerializeField] private IDamageable entityLogic; // The entity should have a script that implements the Damageable interface

    private void Start()
    {
        entityLogic = entity.GetComponent<IDamageable>();
        slider.maxValue = entityLogic.HitPoints;
        slider.value = entityLogic.HitPoints;
    }

    private void Update()
    {
        if (entity == null) return;
        // Update slider to reflect the entity's current HitPoints
        slider.value = entityLogic.HitPoints;

        // Always position the healthbar next to the entity. The exact position can be controlled by the offset
        Vector2 bodyPos = entity.GetComponentInChildren<Rigidbody2D>().position;
        transform.position = (Vector3)bodyPos + offset;

        // Ensure that its rotation stays aligned with camera
        transform.rotation = Camera.main.transform.rotation;
    }
}