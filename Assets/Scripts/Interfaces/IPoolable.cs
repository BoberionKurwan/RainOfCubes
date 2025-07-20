public interface IPoolable
{
    event System.Action<IPoolable> ReturnToPool;

    void ResetThis();
}