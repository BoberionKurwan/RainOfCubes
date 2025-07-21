using UnityEngine;
using TMPro;

public class UIUpdater<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private TMP_Text _counterText;
    [SerializeField] private Pooler<T> _pooler;
    [SerializeField] private string _name;

    private void Start()
    {
        _pooler.CountersUpdated += UpdateCounters;

        UpdateCounters();
    }

    private void OnDestroy()
    {
        _pooler.CountersUpdated -= UpdateCounters;
    }

    private void UpdateCounters()
    {
        _counterText.text = $"{_name}:\nActiveCount:{_pooler.ActiveCount}" +
            $"\nTotalCreated:{_pooler.TotalCreated}" +
            $"\nTotalSpawned:{_pooler.TotalSpawned}";
    }
}