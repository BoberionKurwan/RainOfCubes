using System;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody), typeof(ColorChanger))]
public class SpawnableObject<T> : MonoBehaviour where T : SpawnableObject<T>
{
    protected Renderer _renderer;
    protected Rigidbody _rigidbody;
    protected ColorChanger _colorChanger;
    protected Quaternion _initialRotation;

    public event Action<T> ReturnToPool;

    private void OnValidate()
    {
        if (this is not T)
            throw new InvalidGenericTypeException();
    }

    protected virtual void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _colorChanger = GetComponent<ColorChanger>();
        _initialRotation = transform.rotation;
        _colorChanger.SetColorAsDefault(_renderer, _renderer.material.color);
    }

    private void OnDestroy()
    {
        _colorChanger.RemoveRenderer(_renderer);
    }

    public virtual void Reset()
    {
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        transform.rotation = _initialRotation;
        _colorChanger.SetDefaultColor(_renderer);
    }
    
    protected void InvokeReturnToPool(T obj)
    {
        ReturnToPool?.Invoke(obj);
    }

    private class InvalidGenericTypeException : Exception
    {
        public InvalidGenericTypeException() : base("Generic type type must match the type of the class that inherits this class") { }
    }
}
