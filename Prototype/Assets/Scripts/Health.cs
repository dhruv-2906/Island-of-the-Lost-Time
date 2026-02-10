using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    void Awake() => currentHP = maxHP;

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0) Die();
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
    }

    void Die()
    {
        // Notify game manager of death
        if (CompareTag("Enemy"))
        {
            GameManager gm = GameManager.Instance;
            if (gm != null)
            {
                gm.OnEnemyDefeated(gameObject);
            }
        }
        
        // simple placeholder death
        Destroy(gameObject);
    }
}
