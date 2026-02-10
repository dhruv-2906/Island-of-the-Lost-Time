using UnityEngine;

public class Tree : MonoBehaviour
{
    [Header("Climbing Settings")]
    public Transform climbPosition; // Position where player will be when climbing
    public float climbSpeed = 2f;
    public float interactionRange = 2f;
    
    [Header("Hiding Settings")]
    public bool providesHiding = true;
    
    private PlayerController currentClimber = null;
    private bool playerIsHidden = false;
    
    void Update()
    {
        // If player is climbing, keep them at the climb position
        if (currentClimber != null && climbPosition != null)
        {
            currentClimber.transform.position = Vector3.Lerp(
                currentClimber.transform.position,
                climbPosition.position,
                climbSpeed * Time.deltaTime
            );
        }
    }
    
    /// <summary>
    /// Called when player starts climbing the tree
    /// </summary>
    public void StartClimbing(PlayerController player)
    {
        if (currentClimber != null) return; // Tree already occupied
        
        currentClimber = player;
        playerIsHidden = true;
        
        // Disable player's character controller while climbing
        var cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
        }
        
        Debug.Log("Player is climbing the tree and hiding from enemies!");
    }
    
    /// <summary>
    /// Called when player stops climbing the tree
    /// </summary>
    public void StopClimbing()
    {
        if (currentClimber == null) return;
        
        // Re-enable character controller
        var cc = currentClimber.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = true;
        }
        
        playerIsHidden = false;
        currentClimber = null;
        
        Debug.Log("Player stopped climbing the tree.");
    }
    
    /// <summary>
    /// Check if player is currently hidden in this tree
    /// </summary>
    public bool IsPlayerHidden()
    {
        return playerIsHidden && currentClimber != null;
    }
    
    /// <summary>
    /// Get the player currently climbing this tree
    /// </summary>
    public PlayerController GetClimber()
    {
        return currentClimber;
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
        
        if (climbPosition != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(climbPosition.position, 0.5f);
        }
    }
}
