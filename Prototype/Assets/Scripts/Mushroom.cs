using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [Header("Power Settings")]
    public int powerBoost = 20;
    public int healthBoost = 15;
    public float powerBoostDuration = 30f; // Duration in seconds for temporary power boost
    
    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public bool canBeJumpedOn = true;
    
    private bool isConsumed = false;
    
    /// <summary>
    /// Called when player eats the mushroom
    /// </summary>
    public void Eat(PlayerController player)
    {
        if (isConsumed) return;
        
        isConsumed = true;
        
        // Grant health boost
        var health = player.GetComponent<Health>();
        if (health != null)
        {
            health.currentHP = Mathf.Min(health.currentHP + healthBoost, health.maxHP);
            Debug.Log($"Player gained {healthBoost} health from mushroom!");
        }
        
        // Grant temporary power boost via PlayerController
        player.ApplyPowerBoost(powerBoost, powerBoostDuration);
        
        // Destroy the mushroom after eating
        Destroy(gameObject);
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Auto-eat when player walks into mushroom
        var player = other.GetComponent<PlayerController>();
        if (player != null && !isConsumed)
        {
            Eat(player);
        }
    }
}
