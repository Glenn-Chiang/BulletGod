using UnityEngine;
using UnityEngine.UI;

public class XpDisplay : MonoBehaviour
{
    private PlayerStats playerStats;
    public Slider slider;

    private void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        slider.maxValue = playerStats.xpPerLevel;
        slider.value = playerStats.XP;
    }

    private void Update()
    {
        slider.value = playerStats.XP;
    }
}