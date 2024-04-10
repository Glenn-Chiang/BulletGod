using UnityEngine;
using TMPro;

public class LevelDisplay : MonoBehaviour
{
    private PlayerStats playerStats;
    public TMP_Text levelNumText;

    private void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    private void Update()
    {
        levelNumText.text = playerStats.Level.ToString();
        if (playerStats.Level == PlayerStats.maxLevel)
        {
            levelNumText.text += " (MAX)";
        }
    }
}