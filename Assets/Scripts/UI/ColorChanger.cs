using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Dictionary<Renderer, Color> Colors = new Dictionary<Renderer, Color>();

    public void SetRandomColor(Renderer renderer)
    {
        renderer.material.color = Random.ColorHSV();
    }

    public void SetDefaultColor(Renderer renderer)
    {
        if (Colors.ContainsKey(renderer))
        {
            renderer.material.color = Colors[renderer];
        }
    }

    public void SetColorAsDefault(Renderer renderer, Color color)
    {
        Colors.Add(renderer, color);
    }

    public void RemoveRenderer(Renderer renderer)
    {
        Colors.Remove(renderer);
    }
}