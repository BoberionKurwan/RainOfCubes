using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private float _spawnRadius = 5f;
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private int _poolCapacity = 3;
    [SerializeField] private int _poolMaxSize = 3;

    private ObjectPool<Cube> _pool;
    private Coroutine _spawningCoroutine;
    private int _activeCount = 0;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: CreateCube,
            actionOnGet: EnableCube,
            actionOnRelease: DisableCube,
            actionOnDestroy: DestroyCube,
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        _spawningCoroutine = StartCoroutine(SpawnCubesRoutine());
    }

    private void OnDestroy()
    {
        StopCoroutine(_spawningCoroutine);
    }

    private IEnumerator SpawnCubesRoutine()
    {
        WaitForSeconds waitForSpawnInterval = new WaitForSeconds(_spawnInterval);

        while (enabled)
        {
            yield return waitForSpawnInterval;

            if (_activeCount < _poolMaxSize)
                _pool.Get();
        }
    }

    private Cube CreateCube()
    {
        Cube cube = Instantiate(_cubePrefab);
        cube.Initialize(ReleaseCube);
        return cube;
    }

    private void EnableCube(Cube cube)
    {
        cube.transform.position = GetRandomSpawnPosition();
        cube.ResetCube();
        cube.gameObject.SetActive(true);
        _activeCount++;
    }

    private void DisableCube(Cube cube)
    {
        cube.gameObject.SetActive(false);
        _activeCount--;
    }

    private void DestroyCube(Cube cube)
    {
        Destroy(cube.gameObject);
    }

    private void ReleaseCube(Cube cube)
    {
        _pool.Release(cube);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * _spawnRadius;
        return transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
    }
}
