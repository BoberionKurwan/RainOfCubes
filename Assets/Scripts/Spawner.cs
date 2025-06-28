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
            createFunc: Create,
            actionOnGet: Enable,
            actionOnRelease: Disable,
            actionOnDestroy: Destroy,
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        _spawningCoroutine = StartCoroutine(SpawnRoutine());
    }

    private void OnDestroy()
    {
        StopCoroutine(_spawningCoroutine);
    }

    private IEnumerator SpawnRoutine()
    {
        WaitForSeconds waitForSpawnInterval = new WaitForSeconds(_spawnInterval);

        while (enabled)
        {
            yield return waitForSpawnInterval;

            if (_activeCount < _poolMaxSize)
                _pool.Get();
        }
    }

    private Cube Create()
    {
        Cube cube = Instantiate(_cubePrefab);
        cube.ReturnToPool += Release;
        return cube;
    }

    private void Enable(Cube cube)
    {
        cube.transform.position = GetRandomSpawnPosition();
        cube.ResetCube();
        cube.gameObject.SetActive(true);
        _activeCount++;
    }

    private void Disable(Cube cube)
    {
        cube.gameObject.SetActive(false);
        _activeCount--;
    }

    private void Destroy(Cube cube)
    {
        Destroy(cube.gameObject);
    }

    private void Release(Cube cube)
    {
        _pool.Release(cube);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * _spawnRadius;
        return transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
    }
}
