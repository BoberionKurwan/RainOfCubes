using UnityEngine;

public class Exploder : MonoBehaviour 
{
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _explosionForce = 10f;

    public void Explode(Vector3 centreOfExplosion)
    {
        Collider[] colliders = Physics.OverlapSphere(centreOfExplosion, _explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(_explosionForce, centreOfExplosion, _explosionRadius, 1f, ForceMode.Impulse);
            }
        }
    }
}