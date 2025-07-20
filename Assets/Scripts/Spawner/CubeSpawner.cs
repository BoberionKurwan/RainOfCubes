using System.Collections.Generic;
using UnityEngine;


public class CubeSpawner : Spawner<Cube>
{
    [SerializeField] private float _spawnRadius = 5f;

    protected override Vector3 GetSpawnPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * _spawnRadius;
        return transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
    }
}