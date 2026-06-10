using UnityEngine;
 
public class PowerUp_Health : MonoBehaviour
{
    public int healAmount = 1;
 
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.HealHP(healAmount);
                Destroy(gameObject);
            }
        }
    }
}
 

