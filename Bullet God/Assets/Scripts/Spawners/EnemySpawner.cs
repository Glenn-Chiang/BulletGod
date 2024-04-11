using System;
using System.Collections;
using System.Collections.Generic;
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
        // Prevent spawning in certain range around player position
        var excludeXRange = GetExcludeRange((int)player.transform.position.x);
        var excludeYRange = GetExcludeRange((int)player.transform.position.y);
        var xRange = Enumerable.Range((int)minX, (int)(maxX - minX)).Except(excludeXRange);
        var yRange = Enumerable.Range((int)minY, (int)(maxY - minY)).Except(excludeYRange);

        var rand = new System.Random();
        int xPos = xRange.ToList()[rand.Next(xRange.Count())];
        int yPos = yRange.ToList()[rand.Next(yRange.Count())];
        return new Vector2(xPos, yPos);
    }

    // Get the range of positions to exclude
    private IEnumerable<int> GetExcludeRange(int centrePos)
    {
        // Prevent spawning in certain range around player position
        int offset = 20;
        int lowerBound = centrePos - offset;
        int upperBound = centrePos + offset;
        var excludeRange = Enumerable.Range(lowerBound, upperBound - lowerBound + 1);
        return excludeRange;
    }
}