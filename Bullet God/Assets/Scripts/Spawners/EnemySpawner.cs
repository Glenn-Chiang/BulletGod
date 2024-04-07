using System;
using UnityEngine;

public class EnemySpawner : Spawner
{
    public int numberToSpawn = 100;
    public override int NumberToSpawn => numberToSpawn;

    public WeightedObject[] prefabs;
    public override WeightedObject[] SpawnObjects => prefabs;

    public float spawnInterval = 2;

    private GameManager gameManager;

    protected override void Start()
    {
        base.Start();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.OnGameOver += HandleGameOver;

        // After initial spawning, continue to spawn a new random enemy at set time interval
        InvokeRepeating(nameof(SpawnRandomObject), 0, spawnInterval);
    }

    private void HandleGameOver(object sender, EventArgs e)
    {
        CancelInvoke();
    }
}