using UnityEngine;
using TMPro;

public class UIUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text _cubeCounterText;
    [SerializeField] private TMP_Text _bombCounterText;
    [SerializeField] private Pooler<Cube> _cubePooler;
    [SerializeField] private Pooler<Bomb> _bombPooler;

    private void Start()
    {
        _cubePooler.CountersUpdated += UpdateCubeCounters;
        _bombPooler.CountersUpdated += UpdateBombCounters;
        
        UpdateCubeCounters();
        UpdateBombCounters();
    }

    private void OnDestroy()
    {
        _cubePooler.CountersUpdated -= UpdateCubeCounters;
        _bombPooler.CountersUpdated -= UpdateBombCounters;
    }

    private void UpdateCubeCounters()
    {
        _cubeCounterText.text = $"Cubes:\nActiveCount:{_cubePooler.ActiveCount}" +
            $"\nTotalCreated:{_cubePooler.TotalCreated}" +
            $"\nTotalSpawned:{_cubePooler.TotalSpawned}";
    }

    private void UpdateBombCounters()
    {
        _bombCounterText.text = $"Bombs:\nActiveCount:{_bombPooler.ActiveCount}" +
            $"\nTotalCreated:{_bombPooler.TotalCreated}" +
            $"\nTotalSpawned:{_bombPooler.TotalSpawned}";
    }
}