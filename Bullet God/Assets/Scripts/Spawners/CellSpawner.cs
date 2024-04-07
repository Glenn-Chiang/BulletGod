using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSpawner : Spawner
{
    public int numberToSpawn = 400;
    public override int NumberToSpawn => numberToSpawn;

    public WeightedObject[] prefabs;
    public override WeightedObject[] SpawnObjects => prefabs;
}

