using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody), typeof(SphereCollider))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _explosionForce = 10f;
    [SerializeField] private float _fadeStartTime = 1f;

    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private Coroutine _explodeCoroutine;
    private Material _material;
    private Color _initialColor;
    private float _initialAlpha;

    private float _minExplodeTime = 2f;
    private float _maxExplodeTime = 5f;

    public event Action<Bomb> ReturnToPool;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _material = _renderer.material;
        _initialColor = _material.color;
        _initialAlpha = _initialColor.a;
    }

    private void OnDestroy()
    {
        ReturnToPool = null;
    }

    public void ResetThis()
    {
        _material.color = new Color(_initialColor.r, _initialColor.g, _initialColor.b, _initialAlpha);
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        if (_explodeCoroutine != null)
        {
            StopCoroutine(_explodeCoroutine);
            _explodeCoroutine = null;
        }
    }

    public void Activate()
    {
        _explodeCoroutine = StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator ExplodeAfterDelay()
    {
        float explodeTime = UnityEngine.Random.Range(_minExplodeTime, _maxExplodeTime);
        float elapsedTime = 0f;

        while (elapsedTime < explodeTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / explodeTime;

            Color currentColor = _initialColor;

            if (elapsedTime >= _fadeStartTime)
            {
                float fadeDuration = explodeTime - _fadeStartTime;
                float fadeProgress = (elapsedTime - _fadeStartTime) / fadeDuration;
                currentColor.a = Mathf.Lerp(_initialAlpha, 0f, fadeProgress);
            }

            _material.color = currentColor;

            yield return null;
        }

        Explode();
        ReturnToPool?.Invoke(this);

    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Rigidbody>(out var rigidbody))
            {
                rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, 1f, ForceMode.Impulse);
            }
        }
    }
}
