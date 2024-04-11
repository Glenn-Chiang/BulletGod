using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TMP_Text scoreText;

    private void Start()
    {
        scoreText.text += $" {playerStats.Score}";
    }
}