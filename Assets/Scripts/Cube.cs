using System;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public event Action<Cube> CollidedWithPlatform;

    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private bool _hasCollided;

    public void Initialize(Action<Cube> collisionHandler)
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
        CollidedWithPlatform += collisionHandler;
    }

    public void ResetCube()
    {
        _hasCollided = false;
        ColorChanger.SetDefaultColor(_renderer);
        _rigidbody.linearVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasCollided || !collision.gameObject.CompareTag("Platform"))
            return;

        _hasCollided = true;
        ColorChanger.SetRandomColor(_renderer);
        CollidedWithPlatform?.Invoke(this);
    }

    private void OnDestroy()
    {
        CollidedWithPlatform = null;
    }
}