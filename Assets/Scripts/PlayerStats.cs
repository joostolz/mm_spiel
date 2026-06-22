using UnityEngine;
 
public class PlayerStats : MonoBehaviour
{
    public int maxHP = 3;
    public int currentHP;
 
    void Start()
    {
        currentHP = maxHP;
    }
 
    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log("HP: " + currentHP);
        if (currentHP <= 0) Debug.Log("Game Over");
    }
 
    public void HealHP(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        Debug.Log("HP nach Heilung: " + currentHP);
    }
}
 

