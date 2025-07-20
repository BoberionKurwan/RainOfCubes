using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pooler<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;

    protected ObjectPool<T> _pool;

    public int TotalCreated { get; private set; }
    public int TotalSpawned { get; private set; }
    public int ActiveCount { get; private set; }

    public event System.Action CountersUpdated;

    private void Awake()
    {
        _pool = new ObjectPool<T>(
            createFunc: Create,
            actionOnGet: Enable,
            actionOnRelease: Disable,
            actionOnDestroy: Destroy,
            collectionCheck: true);
    }

    private T Create()
    {
        T obj = Instantiate(_prefab);

        TotalCreated++;
        CountersUpdated?.Invoke();

        return obj;
    }

    private void Enable(T obj)
    {
        obj.gameObject.SetActive(true);

        ActiveCount++;
        TotalSpawned++;
        CountersUpdated?.Invoke();
    }

    private void Disable(T obj)
    {
        obj.gameObject.SetActive(false);

        ActiveCount--;
        CountersUpdated?.Invoke();
    }

    private void Destroy(T obj)
    {
        Destroy(obj.gameObject);
    }

    public T GetObject()
    {
        return _pool.Get();
    }

    public void Release(T obj)
    {
        _pool.Release(obj);
    }
}
