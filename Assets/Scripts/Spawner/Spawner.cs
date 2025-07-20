using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour, IPoolable
{
    [SerializeField] private T _prefab;
    [SerializeField] private float _spawnInterval = 1f;

    protected ObjectPool<T> _pool;
    private Coroutine _spawningCoroutine;
    private int _activeCount = 0;

    private void Awake()
    {
        _pool = new ObjectPool<T>(
            createFunc: Create,
            actionOnGet: Enable,
            actionOnRelease: Disable,
            actionOnDestroy: Destroy,
            collectionCheck: true);
    }

    private void Start()
    {
        _spawningCoroutine = StartCoroutine(SpawnRoutine());
    }

    private void OnDestroy()
    {
        StopCoroutine(_spawningCoroutine);
    }

    protected virtual IEnumerator SpawnRoutine()
    {
        WaitForSeconds waitForSpawnInterval = new WaitForSeconds(_spawnInterval);

        while (enabled)
        {
            yield return waitForSpawnInterval;

            _pool.Get();
        }
    }

    private T Create()
    {
        T obj = Instantiate(_prefab);
        obj.ReturnToPool += (poolable) => Release(poolable as T);
        return obj;
    }

    protected virtual void Enable(T obj)
    {
        obj.transform.position = GetSpawnPosition();
        obj.ResetThis();
        obj.gameObject.SetActive(true);
        _activeCount++;
    }

    private void Disable(T obj)
    {
        obj.gameObject.SetActive(false);
        _activeCount--;
    }

    private void Destroy(T obj)
    {
        Destroy(obj.gameObject);
    }

    private void Release(T obj)
    {
        _pool.Release(obj);
    }

    protected virtual Vector3 GetSpawnPosition()
    {
        return transform.position;
    }
}
