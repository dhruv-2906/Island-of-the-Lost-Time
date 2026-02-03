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

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
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
}
