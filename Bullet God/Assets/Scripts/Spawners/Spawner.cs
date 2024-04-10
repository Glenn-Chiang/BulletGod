using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    public static float minX = WorldMap.minX; // Leftmost spawn position
    public static float maxX = WorldMap.maxX; // Rightmost spawn position
    public static float minY = WorldMap.minY; // Bottommost spawn position
    public static float maxY = WorldMap.maxY; // Topmost spawn position

    public abstract int NumberToSpawn { get; }

    // Array of different entities that can be spawned by this spawner.
    // Each GameObject must be assigned a weight that determines its probability of being spawned.
    public abstract WeightedObject[] SpawnObjects { get; } 

    [Serializable, Inspectable]
    public class WeightedObject
    {
        public GameObject go;
        public float weight;
    }

    protected virtual void Start()
    {
        for (int i = 0; i < NumberToSpawn; i++)
        {
            SpawnRandom();
        }
    }

    protected void SpawnRandom()
    {
        Vector2 spawnPosition = GetRandomPosition();
        GameObject objectToSpawn = GetRandomObject();
        Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
    }

    private GameObject GetRandomObject()
    {
        double randomNumber = new System.Random().NextDouble();
        foreach (var weightedObject in SpawnObjects)
        {
            randomNumber -= weightedObject.weight;
            if (randomNumber < 0)
            {
                var objectToSpawn = weightedObject.go;
                return objectToSpawn;
            }
        }
        // If weights did not add up to 1, return the first item as default
        return SpawnObjects[0].go;
    }

    protected virtual Vector2 GetRandomPosition()
    {
        float xPos = UnityEngine.Random.Range(minX, maxX);
        float yPos = UnityEngine.Random.Range(minY, maxY);
        return new Vector2(xPos, yPos);
    }
}
