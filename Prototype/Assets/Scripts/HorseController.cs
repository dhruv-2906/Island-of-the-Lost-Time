using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HorseController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float gallopSpeed = 12f;
    public Transform mountPoint; // where player will parent to

    Rigidbody rb;
    PlayerController rider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rider == null) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 dir = (transform.right * x + transform.forward * z).normalized;
        float speed = Input.GetKey(KeyCode.LeftShift) ? gallopSpeed : walkSpeed;

        rb.MovePosition(transform.position + dir * speed * Time.deltaTime);

        // Dismount via E handled on player

        // Attack while riding
        if (Input.GetMouseButtonDown(0))
        {
            var melee = rider.GetComponent<MeleeAttack>();
            if (melee != null) melee.Attack();
        }
    }

    public void Ride(PlayerController p)
    {
        rider = p;
    }

    public void Dismount()
    {
        rider = null;
    }
}
