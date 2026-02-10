using UnityEngine;
using System.Collections;

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
        
        // Grant temporary power boost
        var melee = player.GetComponent<MeleeAttack>();
        if (melee != null)
        {
            player.StartCoroutine(ApplyTemporaryPowerBoost(melee));
        }
        
        // Destroy the mushroom after eating
        Destroy(gameObject);
    }
    
    /// <summary>
    /// Apply temporary power boost to the player's melee attack
    /// </summary>
    private IEnumerator ApplyTemporaryPowerBoost(MeleeAttack melee)
    {
        int originalDamage = melee.damage;
        melee.damage += powerBoost;
        Debug.Log($"Player gained {powerBoost} attack power from mushroom for {powerBoostDuration} seconds!");
        
        yield return new WaitForSeconds(powerBoostDuration);
        
        melee.damage = originalDamage;
        Debug.Log("Mushroom power boost has worn off!");
    }
    
    /// <summary>
    /// Called when player jumps on the mushroom
    /// </summary>
    public void BouncePlayer(CharacterController cc)
    {
        if (!canBeJumpedOn) return;
        
        // Apply upward force by modifying velocity
        // This will be handled in PlayerController
        Debug.Log("Player bounced on mushroom!");
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
