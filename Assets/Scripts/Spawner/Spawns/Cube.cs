using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody))]
public class Cube : SpawnableObject<Cube>
{
    private Coroutine _returnCoroutine;

    private bool _hasCollided;
    private float _minDelay = 1f;
    private float _maxDelay = 3f;
        
    private void OnCollisionEnter(Collision collision)
    {
        if (_hasCollided || !collision.gameObject.TryGetComponent<Platform>(out _))
            return;

        _hasCollided = true;
        ColorChanger.SetRandomColor(_renderer);
        _returnCoroutine = StartCoroutine(ReturnToPoolAfterDelay());
    }

    public override void Reset()
    {
        base.Reset();

        _hasCollided = false;
        ColorChanger.SetDefaultColor(_renderer);

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
            InvokeReturnToPool(this);
        }
    }
}