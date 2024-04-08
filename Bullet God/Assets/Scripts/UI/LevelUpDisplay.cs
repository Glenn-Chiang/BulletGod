using System;
using System.Collections;
using UnityEngine;

public class LevelUpDisplay : MonoBehaviour
{
    private PlayerStats playerStats;
    [SerializeField] private GameObject levelUpScreen;

    private void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        playerStats.OnLevelUp += HandleLevelUp;

    }

    private void HandleLevelUp(object sender, EventArgs e)
    {
        StartCoroutine(ShowLevelUp());
    }

    private IEnumerator ShowLevelUp()
    {
        levelUpScreen.SetActive(true);
        yield return new WaitForSeconds(1);
        levelUpScreen.SetActive(false);
    }
}