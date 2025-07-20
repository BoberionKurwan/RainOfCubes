using UnityEngine;
using TMPro;

public class TextHandler
{
    [SerializeField] private TMP_Text _text;

    public void UpdateText(int count)
    {
        _text.text = $"Count: {count}";
    }
}