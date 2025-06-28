using System;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public event Action<Cube, Collision> CollisionEnter;

    private void OnCollisionEnter(Collision collision)
    {
        ColorChanger.SetColorRed(GetComponent<Renderer>());
        CollisionEnter?.Invoke(this, collision);
    }
}