using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public static void SetColorRed(Renderer renderer)
    {
        renderer.material.color = Color.red;
    }

    public static void SetDefaultColor(Renderer renderer)
    {
        renderer.material.color = Color.white;
    }
}