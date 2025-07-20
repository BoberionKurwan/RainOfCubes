using System;
using System.Collections;
using UnityEngine;


public class CubeSpawner : Spawner<Cube>
{
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _spawnRadius = 5f;

    public event Action<Vector3> CubeReleased;

    private Coroutine _spawnCoroutine;

    private void Start()
    {
        _spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private void SetSpawnPosition(Cube cube)
    {
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * _spawnRadius;
        cube.transform.position = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
    }

    private void Release(Cube cube)
    {
        cube.ReturnToPool -= Release;
        cube.ResetThis();
        Pooler.Release(cube);
        CubeReleased?.Invoke(cube.transform.position); 
    }

    private IEnumerator SpawnRoutine()
    {
        WaitForSeconds waitForSpawnInterval = new WaitForSeconds(_spawnInterval);

        while (enabled)
        {
            yield return waitForSpawnInterval;

            Cube cube = Pooler.GetObject();
            cube.ReturnToPool += Release;
            SetSpawnPosition(cube);            
        }
    }
}