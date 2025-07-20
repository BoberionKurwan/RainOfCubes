using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody))]
public class Cube : MonoBehaviour, IPoolable
{
    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private Coroutine _returnCoroutine;
    private Quaternion _initialRotation;

    private bool _hasCollided;
    private float _minDelay = 1f;
    private float _maxDelay = 3f;

    public event Action<IPoolable> ReturnToPool;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _initialRotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasCollided || !collision.gameObject.TryGetComponent<Platform>(out _))
            return;

        _hasCollided = true;
        ColorChanger.SetRandomColor(_renderer);
        _returnCoroutine = StartCoroutine(ReturnToPoolAfterDelay());
    }

    private void OnDestroy()
    {
        ReturnToPool = null;
    }

    public void ResetThis()
    {
        _hasCollided = false;
        ColorChanger.SetDefaultColor(_renderer);
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        transform.rotation = _initialRotation;

        if (_returnCoroutine != null)
        {
            StopCoroutine(_returnCoroutine);
            _returnCoroutine = null;
        }
    }

    private IEnumerator ReturnToPoolAfterDelay()
    {
        float delay = UnityEngine.Random.Range(_minDelay, _maxDelay);
        WaitForSeconds waitForDelay = new WaitForSeconds(delay);

        yield return waitForDelay;

        if (gameObject.activeInHierarchy)
        {
            GameEvents.CubeReturnedToPool(transform.position);
            ReturnToPool?.Invoke(this);
        }
    }
}
