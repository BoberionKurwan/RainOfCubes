using UnityEngine;

public class TransparencyFader : MonoBehaviour 
{
    public void Fade(float progress, Material material)
    {
        Color newColor = material.color;
        newColor.a = Mathf.Lerp(material.color.a, 0f, progress);
        material.color = newColor;
    }
}