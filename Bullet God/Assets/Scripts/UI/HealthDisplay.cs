using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    private PlayerStats playerStats;
    public Slider slider;

    private void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        slider.maxValue = playerStats.maxHealth.value;
        slider.value = playerStats.maxHealth.value;   
    }

    private void Update()
    {
        slider.value = playerStats.Health;
    }
}