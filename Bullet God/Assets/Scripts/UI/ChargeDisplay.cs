using UnityEngine;
using UnityEngine.UI;

public class ChargeDisplay : MonoBehaviour
{
    private PlayerStats playerStats;
    public Slider slider;

    private void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        slider.maxValue = playerStats.maxCharges;
        slider.value = playerStats.ChargeCount;
    }

    private void Update()
    {
        slider.value = playerStats.ChargeCount;
    }
}