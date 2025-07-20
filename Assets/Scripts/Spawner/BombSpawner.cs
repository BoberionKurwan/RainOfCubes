using System;
using System.Collections;
using UnityEngine;

public class BombSpawner : Spawner<Bomb>
{
    private Vector3 _spawnPosition;

    private void OnEnable()
    {
        GameEvents.OnCubeReturnedToPool += SpawnAtPosition;
    }

    private void OnDisable()
    {
        GameEvents.OnCubeReturnedToPool -= SpawnAtPosition;
    }

    protected override void Enable(Bomb bomb)
    {
        base.Enable(bomb);
        bomb.Activate();
    }

    protected override IEnumerator SpawnRoutine()
    {
        yield break;
    }

    protected override Vector3 GetSpawnPosition()
    {
        return _spawnPosition;
    }

    private void SpawnAtPosition(Vector3 position)
    {
        _spawnPosition = position;
        _pool.Get();
    }

}
public static class GameEvents
{
    public static event Action<Vector3> OnCubeReturnedToPool;

    public static void CubeReturnedToPool(Vector3 position)
    {
        OnCubeReturnedToPool?.Invoke(position);
    }
}