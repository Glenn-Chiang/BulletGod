using System;
using UnityEngine;

public class GameOverDisplay : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private GameObject gameOverScreen;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.OnGameOver += ShowGameOver;
    }

    private void ShowGameOver(object sender, EventArgs e)
    {
        gameOverScreen.SetActive(true);
    }
}