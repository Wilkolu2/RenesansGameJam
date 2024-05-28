using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit player");
        }

        Destroy(gameObject);
    }
}