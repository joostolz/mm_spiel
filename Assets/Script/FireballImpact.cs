using UnityEngine;

public class FireballImpact : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Destroy the fireball if it touches the floor or player
        // (You can filter via tags like collision.gameObject.CompareTag("Ground"))
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Backup safety if using trigger zones instead of hard physics colliders
        Destroy(gameObject);
    }
}
