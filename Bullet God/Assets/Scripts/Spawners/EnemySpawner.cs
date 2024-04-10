using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemySpawner : Spawner
{
    [SerializeField] private int numberToSpawn = 100;
    public override int NumberToSpawn => numberToSpawn;

    [SerializeField] private WeightedObject[] prefabs;
    public override WeightedObject[] SpawnObjects => prefabs;

    [SerializeField] private float spawnInterval = 2; // How often to spawn a new enemy
    [SerializeField] private float minSpawnInterval = 0.2f;
    [SerializeField] private float incrementInterval = 10; // How often (in seconds) to increase the spawn rate
    [SerializeField] private float factor = 0.9f; // Factor by which spawnInterval will be multiplied during each increment

    private GameManager gameManager;

    protected override void Start()
    {
        base.Start();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.OnGameOver += HandleGameOver;

        StartCoroutine(SpawnRoutine());
        InvokeRepeating(nameof(IncrementSpawnRate), 0, incrementInterval);
    }

    // Spawn a new randomy enemy at interval
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnRandom();
        }
    }

    private void IncrementSpawnRate()
    {
        spawnInterval = Math.Max(spawnInterval * factor, minSpawnInterval);
    }

    private void HandleGameOver(object sender, EventArgs e)
    {
        StopCoroutine(SpawnRoutine());
        CancelInvoke();
    }

    protected override Vector2 GetRandomPosition()
    {
        // Prevent spawning in certain range around origin, i.e. start position of player
        var excludedRange = Enumerable.Range(-20, 20);
        var xRange = Enumerable.Range((int)minX, (int)(maxX - minX)).Except(excludedRange);
        var yRange = Enumerable.Range((int)minY, (int)(maxY - minY)).Except(excludedRange);

        var rand = new System.Random();
        int xPos = xRange.ToList()[rand.Next(xRange.Count())];
        int yPos = yRange.ToList()[rand.Next(yRange.Count())];
        return new Vector2(xPos, yPos);
    }
}