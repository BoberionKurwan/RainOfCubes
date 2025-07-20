using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody), typeof(SphereCollider))]
[RequireComponent(typeof(Exploder))]
public class Bomb : SpawnableObject<Bomb>
{
    [SerializeField] private float _fadeStartTime = 1f;

    private Coroutine _explodeCoroutine;
    private Material _material;
    private Exploder _exploder;

    private float _minExplodeTime = 2f;
    private float _maxExplodeTime = 5f;


    protected override void Awake()
    {
        base.Awake();
        _exploder = GetComponent<Exploder>();

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

        yield return ColorChanger.FadeIntoTransparency(_material, _material.color, fadeDuration);

        _exploder.Explode(transform.position);
        InvokeReturnToPool(this);
    }
}
