using UnityEngine;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected Pooler<T> Pooler;

    // Add UI
}