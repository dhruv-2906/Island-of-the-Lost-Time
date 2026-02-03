using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public float range = 2f;
    public int damage = 25;
    public LayerMask hitMask;
    public GameObject hitVFX;

    public void Attack()
    {
        Vector3 origin = transform.position + Vector3.up * 1.0f;
        Collider[] hits = Physics.OverlapSphere(origin, range, hitMask);
        foreach (var c in hits)
        {
            var health = c.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            if (hitVFX != null)
            {
                Instantiate(hitVFX, c.ClosestPoint(origin), Quaternion.identity);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 1.0f, range);
    }
}
