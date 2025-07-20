using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CubeSpawner))]
public class BombSpawner : Spawner<Bomb>
{
    private CubeSpawner _cubeSpawner;

    private void Awake()
    {
        _cubeSpawner = GetComponent<CubeSpawner>();
    }

    private void Start()
    {
        _cubeSpawner.CubeReleased += SpawnAtPosition;
    }

    private void SpawnAtPosition(Vector3 position)
    {
        Bomb bomb = Pooler.GetObject();
        bomb.ReturnToPool += Release;
        bomb.transform.position = position;
        bomb.Activate();
    }

    private void Release(Bomb bomb)
    {
        bomb.ReturnToPool -= Release;
        bomb.ResetThis();
        Pooler.Release(bomb);
    }
}