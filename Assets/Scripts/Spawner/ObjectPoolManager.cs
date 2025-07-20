using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    private GameObject _emptyHolder;

    private static GameObject _cubesEmpty;
    private static GameObject _bombsEmpty;

    private static Dictionary<GameObject, ObjectPool<GameObject>> _objectPools;
    private static Dictionary<GameObject, GameObject> _cloneToPrefabMap;

    public enum PoolType
    {
        GameObjects,
        Cubes,
        Bombs
    }

    public static PoolType PoolingType;

    public static T SpawnObject<T>(T typePrefab, Vector3 position, Quaternion rotation, PoolType poolType = PoolType.GameObjects) where T : Component
    {
        return SpawnObject<T>(typePrefab.gameObject, position, rotation, poolType);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 position, Quaternion rotation, PoolType poolType = PoolType.GameObjects)
    {
        return SpawnObject<GameObject>(objectToSpawn, position, rotation, poolType);
    }

    public static void ReturnObjectToPool(GameObject obj, PoolType poolType = PoolType.GameObjects)
    {
        if (_cloneToPrefabMap.TryGetValue(obj, out GameObject prefab))
        {
            GameObject parentObject = SetParentObject(poolType);

            if (obj.transform.parent != parentObject.transform)
            {
                obj.transform.SetParent(parentObject.transform);
            }

            if (_objectPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                pool.Release(obj);
            }
        }
        else
        {
            Debug.LogWarning("Trying to return an object that is not pooled: " + obj.name);
        }
    }

    private void Awake()
    {
        _objectPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
        _cloneToPrefabMap = new Dictionary<GameObject, GameObject>();

        SetupEmpties();
    }

    private void SetupEmpties()
    {
        _emptyHolder = new GameObject("Object Pools");

        _cubesEmpty = new GameObject("Cubes");
        _cubesEmpty.transform.SetParent(_emptyHolder.transform);

        _bombsEmpty = new GameObject("Bombs");
        _cubesEmpty.transform.SetParent(_emptyHolder.transform);
    }

    private static void CreatePool(GameObject prefab, Vector3 position, Quaternion rotation, PoolType poolType)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => Create(prefab, position, rotation, poolType),
            actionOnGet: Get,
            actionOnRelease: Release,
            actionOnDestroy: Destroy);

        _objectPools.Add(prefab, pool);
    }

    private static GameObject Create(GameObject prefab, Vector3 position, Quaternion rotation, PoolType poolType = PoolType.GameObjects)
    {
        prefab.SetActive(false);

        GameObject obj = Instantiate(prefab);

        GameObject parentObject = SetParentObject(poolType);
        obj.transform.SetParent(parentObject.transform);

        return obj;
    }

    private static void Get(GameObject obj)
    {
        obj.SetActive(true);
    }

    private static void Release(GameObject obj)
    {
        obj.SetActive(false);
    }

    private static void Destroy(GameObject obj)
    {
        if (_cloneToPrefabMap.ContainsKey(obj))
            _cloneToPrefabMap.Remove(obj);
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.Cubes:
                return _cubesEmpty;
            case PoolType.Bombs:
                return _bombsEmpty;
            default:
                return null;
        }
    }

    private static T SpawnObject<T>(GameObject objectToSpawn, Vector3 position, Quaternion rotation, PoolType poolType = PoolType.GameObjects) where T : Object
    {
        if (_objectPools.ContainsKey(objectToSpawn) == false)
            CreatePool(objectToSpawn, position, rotation, poolType);

        GameObject obj = _objectPools[objectToSpawn].Get();

        if (obj == null)
        {
            if (_cloneToPrefabMap.ContainsKey(obj))
            {
                _cloneToPrefabMap.Add(obj, objectToSpawn);
            }

            obj.SetActive(true);

            if (typeof(T) == typeof(GameObject))
            {
                return obj as T;
            }

            T component = obj.GetComponent<T>();

            if (component == null)
            {
                Debug.LogError($"Object {objectToSpawn.name} doesn't have component of type {typeof(T)}");
                return null;
            }

            return component;
        }

        return null;
    }

}