using UnityEngine;
 
public class PowerUp_Speed : MonoBehaviour
{
    public float speedMultiplier = 1.5f;
    public float duration = 5f;
 
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController controller = other.GetComponent<PlayerController>();
            if (controller != null)
            {
                StartCoroutine(SpeedBoostRoutine(controller));
                Destroy(gameObject);
            }
        }
    }
 
    private System.Collections.IEnumerator SpeedBoostRoutine(PlayerController controller)
    {
        controller.movementSpeed *= speedMultiplier;
        yield return new WaitForSeconds(duration);
        controller.movementSpeed /= speedMultiplier;
    }
}

