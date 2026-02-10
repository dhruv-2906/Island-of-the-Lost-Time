using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float gallopSpeed = 8f;
    public float gravity = -9.81f;
    public Transform mountSocket;

    CharacterController cc;
    Vector3 velocity;
    public bool isMounted { get; private set; }
    public HorseController horse;
    
    // Climbing/Hiding state
    public bool isClimbing { get; private set; }
    private Tree currentTree;
    
    // Mushroom jumping
    public float mushroomJumpBoost = 10f;
    public float mushroomDetectionRange = 2f;
    public float mushroomRaycastOffset = 0.1f;
    public float treeInteractionRange = 3f;
    
    // Power boost tracking
    private Coroutine activePowerBoostCoroutine;
    private int baseDamage;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        
        // Store base damage for power boost system
        var melee = GetComponent<MeleeAttack>();
        if (melee != null)
        {
            baseDamage = melee.damage;
        }
    }

    void Update()
    {
        // If climbing, handle tree climbing controls
        if (isClimbing)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StopClimbing();
            }
            return;
        }
        
        if (isMounted)
        {
            // When mounted, control is forwarded to the horse
            if (Input.GetKeyDown(KeyCode.E))
            {
                Dismount();
            }
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        float speed = Input.GetKey(KeyCode.LeftShift) ? gallopSpeed : walkSpeed;

        cc.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

        // Mount
        if (Input.GetKeyDown(KeyCode.E) && horse != null)
        {
            float dist = Vector3.Distance(transform.position, horse.transform.position);
            if (dist < 3f)
            {
                Mount();
            }
        }

        // Attack -> forwards to MeleeAttack component
        if (Input.GetMouseButtonDown(0))
        {
            var melee = GetComponent<MeleeAttack>();
            if (melee != null) melee.Attack();
        }

        // Rally horn (H)
        if (Input.GetKeyDown(KeyCode.H))
        {
            var gm = FindObjectOfType<GameManager>();
            if (gm != null) gm.RallySquad(transform.position);
        }
        
        // Climb tree (F)
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryClimbNearbyTree();
        }
        
        // Jump on mushroom (Space when near mushroom)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryJumpOnMushroom();
        }
    }

    void Mount()
    {
        isMounted = true;
        // parent to horse's mount socket and reposition
        transform.SetParent(horse.mountPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        cc.enabled = false;
        horse.Ride(this);
    }

    void Dismount()
    {
        isMounted = false;
        transform.SetParent(null);
        cc.enabled = true;
        horse.Dismount();
    }
    
    void TryClimbNearbyTree()
    {
        // Find nearby trees
        Collider[] colliders = Physics.OverlapSphere(transform.position, treeInteractionRange);
        foreach (var col in colliders)
        {
            Tree tree = col.GetComponent<Tree>();
            if (tree != null)
            {
                bool success = tree.StartClimbing(this);
                if (success)
                {
                    isClimbing = true;
                    currentTree = tree;
                    return;
                }
            }
        }
    }
    
    void StopClimbing()
    {
        if (currentTree != null)
        {
            currentTree.StopClimbing();
            currentTree = null;
        }
        isClimbing = false;
    }
    
    void TryJumpOnMushroom()
    {
        // Start raycast slightly above ground to avoid colliding with player's own collider
        Vector3 rayStart = transform.position + Vector3.up * mushroomRaycastOffset;
        RaycastHit hit;
        if (Physics.Raycast(rayStart, Vector3.down, out hit, mushroomDetectionRange))
        {
            Mushroom mushroom = hit.collider.GetComponent<Mushroom>();
            if (mushroom != null && mushroom.canBeJumpedOn)
            {
                // Apply upward velocity for bounce effect
                velocity.y = mushroomJumpBoost;
                Debug.Log("Player bounced on mushroom!");
            }
        }
    }
    
    /// <summary>
    /// Apply a temporary power boost to player's melee attack
    /// </summary>
    public void ApplyPowerBoost(int boost, float duration)
    {
        // Cancel any existing power boost
        if (activePowerBoostCoroutine != null)
        {
            StopCoroutine(activePowerBoostCoroutine);
        }
        
        activePowerBoostCoroutine = StartCoroutine(PowerBoostCoroutine(boost, duration));
    }
    
    private IEnumerator PowerBoostCoroutine(int boost, float duration)
    {
        var melee = GetComponent<MeleeAttack>();
        if (melee != null)
        {
            // Set damage to base + boost (prevents stacking issues)
            melee.damage = baseDamage + boost;
            Debug.Log($"Player gained {boost} attack power for {duration} seconds!");
            
            yield return new WaitForSeconds(duration);
            
            // Restore to base damage
            melee.damage = baseDamage;
            Debug.Log("Power boost has worn off!");
        }
        
        activePowerBoostCoroutine = null;
    }
    
    /// <summary>
    /// Check if player is currently hidden from enemies
    /// </summary>
    public bool IsHidden()
    {
        return isClimbing && currentTree != null && currentTree.IsPlayerHidden();
    }
}
