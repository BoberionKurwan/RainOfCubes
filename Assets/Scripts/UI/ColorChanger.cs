using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public static Dictionary<Renderer, Color> colors = new Dictionary<Renderer, Color>();

    public static void SetRandomColor(Renderer renderer)
    {
        renderer.material.color = Random.ColorHSV();
    }

    public static void SetDefaultColor(Renderer renderer)
    {
        if (colors.ContainsKey(renderer))
        {
            renderer.material.color = colors[renderer];
        }
    }

    public static void SetColorAsDefault(Renderer renderer, Color color)
    {
        colors.Add(renderer, color);
    }

    public static IEnumerator FadeIntoTransparency(Material material, Color initialColor, float fadeDuration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeDuration;

            Color newColor = initialColor;
            newColor.a = Mathf.Lerp(initialColor.a, 0f, progress);
            material.color = newColor;

            yield return null;
        }

        Color finalColor = initialColor;
        finalColor.a = 0f;
        material.color = finalColor;
    }


    public static void RemoveRenderer(Renderer renderer)
    {
        colors.Remove(renderer);
    }
}