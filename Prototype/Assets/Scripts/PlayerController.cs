using UnityEngine;

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
    public float treeInteractionRange = 3f;

    void Start()
    {
        cc = GetComponent<CharacterController>();
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
        // Check if there's a mushroom below the player
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, mushroomDetectionRange))
        {
            Mushroom mushroom = hit.collider.GetComponent<Mushroom>();
            if (mushroom != null && mushroom.canBeJumpedOn)
            {
                // Apply upward velocity for bounce effect
                velocity.y = mushroomJumpBoost;
                mushroom.BouncePlayer(cc);
            }
        }
    }
    
    /// <summary>
    /// Check if player is currently hidden from enemies
    /// </summary>
    public bool IsHidden()
    {
        return isClimbing && currentTree != null && currentTree.IsPlayerHidden();
    }
}
