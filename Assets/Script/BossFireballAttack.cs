using UnityEngine;

public class BossFireballAttack : MonoBehaviour
{
    [Header("Fireball Prefab Settings")]
    public GameObject fireballPrefab;
    public Transform mouthTransform;

    [Header("Target Tracking")]
    [Tooltip("Drag your player character object into this slot.")]
    public Transform playerTarget;
    [Tooltip("Speed of the fired projectile.")]
    public float fireballSpeed = 10f;

    [Header("Attack Speed Ramping")]
    [Tooltip("Time between shots when boss is at 100% health.")]
    public float slowFireRate = 3f;
    [Tooltip("Time between shots when boss is near 0% health.")]
    public float fastFireRate = 0.5f;

    private BossHealth bossHealth;
    private float fireTimer;

    void Start()
    {
        // Automatically find the BossHealth script on this same object
        bossHealth = GetComponent<BossHealth>();
        
        // Safety check to look for the player if forgotten in Inspector
        if (playerTarget == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null) playerTarget = player.transform;
        }

        // Shoot instantly right when the game starts/boss engages
        fireTimer = slowFireRate;
    }

    void Update()
    {
        // 3. CRITERIA: Stop shooting entirely if health is 0 or missing
        if (bossHealth == null || bossHealth.GetHealthPercentage() <= 0)
        {
            return; 
        }

        // 2. CRITERIA: Blend smoothly between slow and fast fire rates based on health
        float currentHealthPercent = bossHealth.GetHealthPercentage();
        float currentFireRate = Mathf.Lerp(fastFireRate, slowFireRate, currentHealthPercent);

        fireTimer += Time.deltaTime;
        if (fireTimer >= currentFireRate)
        {
            ShootFireballAtPlayer();
            fireTimer = 0f; // Reset timer loop
        }
    }

    void ShootFireballAtPlayer()
    {
        if (fireballPrefab == null || mouthTransform == null || playerTarget == null) return;

        // Spawn the fireball at the mouth position
        GameObject spawnedFireball = Instantiate(fireballPrefab, mouthTransform.position, Quaternion.identity);

        // 1. CRITERIA: Calculate direction toward the character's exact position right now
        Vector3 targetDirection = (playerTarget.position - mouthTransform.position).normalized;

        // Give the fireball a physics push in that direction
        Rigidbody rb = spawnedFireball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = targetDirection * fireballSpeed;
        }
    }
}