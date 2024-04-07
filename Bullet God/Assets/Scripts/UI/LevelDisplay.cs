using UnityEngine;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour
{
    private PlayerStats playerStats;
    public Text levelNumText;

    private void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    private void Update()
    {
        levelNumText.text = playerStats.Level.ToString();
    }
}