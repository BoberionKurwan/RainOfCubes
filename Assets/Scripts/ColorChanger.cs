using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public static void SetRandomColor(Renderer renderer)
    {
        renderer.material.color = Random.ColorHSV();
    }

    public static void SetDefaultColor(Renderer renderer)
    {
        renderer.material.color = Color.white;
    }
}