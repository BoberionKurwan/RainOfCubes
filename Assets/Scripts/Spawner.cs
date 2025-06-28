using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _spawnRadius = 5f;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 3;
    [SerializeField] private int _poolMaxSize = 3;

    private ObjectPool<GameObject> _pool;

    private int _spawnedCount = 0;
    private float minDelay = 5f;
    private float maxDelay = 10f;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => ActionOnRelease(obj),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void ActionOnGet(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        ColorChanger.SetDefaultColor(renderer);

        obj.transform.position = GetRandomSpawnPosition();
        obj.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        obj.SetActive(true);

        _spawnedCount++;

        Cube cube = obj.GetComponent<Cube>();

        if (cube != null)
            cube.CollisionEnter += OnCubeCollision;
    }

    private void ActionOnRelease(GameObject obj)
    {
        obj.SetActive(false);
        _spawnedCount--;
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _repeatRate);
    }

    private void GetCube()
    {
        if (_spawnedCount < _poolMaxSize)
            _pool.Get();
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * _spawnRadius;
        return transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
    }

    private void OnCubeCollision(Cube cube, Collision collision)
    {
        cube.CollisionEnter -= OnCubeCollision;

        StartCoroutine(ReturnToPoolAfterDelay(cube.gameObject));
    }

    private IEnumerator ReturnToPoolAfterDelay(GameObject cube)
    {
        float delay = Random.Range(minDelay, maxDelay);

        yield return new WaitForSeconds(delay);

        if (cube.activeInHierarchy)
        {
            _pool.Release(cube);
        }
    }
}