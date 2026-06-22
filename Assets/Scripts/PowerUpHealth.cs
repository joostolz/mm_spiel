using UnityEngine;
 
public class PowerUp_Health : MonoBehaviour
{
    public int healAmount = 1;
    
    public void die(){
        Destroy(gameObject);
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         PlayerStats stats = other.GetComponent<PlayerStats>();
    //         if (stats != null)
    //         {
    //             stats.HealHP(healAmount);
    //             Destroy(gameObject);
    //         }
    //     }
    // }
}
 

