using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI Settings")]
    public Image healthBarSlider;
    private Transform mainCameraTransform;

    void Start()
    {
        currentHealth = maxHealth;
        mainCameraTransform = Camera.main.transform;
        UpdateHealthUI();
    }

    void Update()
    {
        // Make the health bar rotate to always face the player's camera
        if (healthBarSlider != null)
        {
            healthBarSlider.canvas.transform.LookAt(healthBarSlider.canvas.transform.position + mainCameraTransform.forward);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        
        // Prevent health from going below zero
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            DefeatBoss();
        }
    }

    void UpdateHealthUI()
    {
        if (healthBarSlider != null)
        {
            // Set the slider fill percentage (between 0.0 and 1.0)
            healthBarSlider.fillAmount = currentHealth / maxHealth;
        }
    }

    // THIS IS THE FUNCTION THE ATTACK SCRIPT IS LOOKING FOR:
    public float GetHealthPercentage()
    {
        if (maxHealth <= 0) return 0f;
        return currentHealth / maxHealth;
    }

    void DefeatBoss()
    {
        Debug.Log("Boss has been defeated!");
        Destroy(gameObject);
    }
}