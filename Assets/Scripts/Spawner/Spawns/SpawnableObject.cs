using System;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody))]
public class SpawnableObject<T> : MonoBehaviour where T : SpawnableObject<T>
{
    protected Renderer _renderer;
    protected Rigidbody _rigidbody;
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
        _initialRotation = transform.rotation;
        ColorChanger.SetColorAsDefault(_renderer, _renderer.material.color);
    }

    private void OnDestroy()
    {
        ColorChanger.RemoveRenderer(_renderer);
    }

    public virtual void Reset()
    {
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        transform.rotation = _initialRotation;
        ColorChanger.SetDefaultColor(_renderer);
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
