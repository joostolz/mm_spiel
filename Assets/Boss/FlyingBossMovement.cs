using UnityEngine;

public class FlyingBossMovement : MonoBehaviour
{
    [Header("Hover Settings")]
    public float hoverAmplitude = 0.5f;
    public float hoverSpeed = 2f;

    [Header("Fake Flap/Pulse Settings")]
    [Tooltip("How fast the boss pulses/squashes to simulate flying.")]
    public float pulseSpeed = 6f;
    [Tooltip("How much the boss widens out when 'flapping'.")]
    public float pulseAmount = 0.1f;

    private Vector3 startPosition;
    private Vector3 startScale;

    void Start()
    {
        // Save the starting position and scale
        startPosition = transform.position;
        startScale = transform.localScale;
    }

    void Update()
    {
        // 1. Handle Hovering Up and Down smoothly
        float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // 2. Handle Fake Wing Flapping (Squash and Stretch)
        // This dynamically widens and narrows the alien to make it look alive!
        float scalePulse = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        
        // We stretch the X (width) and slightly compress the Y (height) to look organic
        transform.localScale = new Vector3(
            startScale.x + scalePulse, 
            startScale.y - (scalePulse * 0.5f), 
            startScale.z
        );
    }
}