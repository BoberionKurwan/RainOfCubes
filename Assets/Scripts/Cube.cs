using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private Coroutine _returnCoroutine;
    private Quaternion _initialRotation;

    private bool _hasCollided;
    private float _minDelay = 1f;
    private float _maxDelay = 3f;

    private event Action<Cube> _returnToPool;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _initialRotation = transform.rotation;
    }

    public void Initialize(Action<Cube> collisionHandler)
    {
        _returnToPool += collisionHandler;
    }

    public void ResetCube()
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

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasCollided || collision.gameObject.GetComponent<Platform>() == null) 
            return;

        _hasCollided = true;
        ColorChanger.SetRandomColor(_renderer);
        _returnCoroutine = StartCoroutine(ReturnToPoolAfterDelay());
    }

    private void OnDestroy()
    {
        _returnToPool = null;
    }

    private IEnumerator ReturnToPoolAfterDelay()
    {
        float delay = UnityEngine.Random.Range(_minDelay, _maxDelay);
        WaitForSeconds waitForDelay = new WaitForSeconds(delay);

        yield return waitForDelay;

        if (gameObject.activeInHierarchy)
            _returnToPool?.Invoke(this);
    }
}
