using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody), typeof(SphereCollider))]
[RequireComponent(typeof(Exploder), typeof(TransparencyFader))]
public class Bomb : SpawnableObject<Bomb>
{
    [SerializeField] private float _fadeStartTime = 1f;

        private Coroutine _explodeCoroutine;
    private Material _material;
    private Exploder _exploder;
    private TransparencyFader _transparencyFader;

    private float _minExplodeTime = 2f;
    private float _maxExplodeTime = 5f;


    protected override void Awake()
    {
        base.Awake();
        _exploder = GetComponent<Exploder>();
        _transparencyFader = GetComponent<TransparencyFader>();

        _material = _renderer.material;
    }

    public override void Reset()
    {
        base.Reset();

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
        float fadeDuration = explodeTime - _fadeStartTime;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeDuration;

           _transparencyFader.Fade(progress, _material);

            yield return null;
        }

        _exploder.Explode(transform.position);
        InvokeReturnToPool(this);
    }
}
