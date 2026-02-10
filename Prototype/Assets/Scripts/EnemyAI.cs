using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Advanced enemy AI for the Island of the Lost Time adventure game
/// Enemies can patrol, chase, attack, and have different behaviors
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Type")]
    [Tooltip("Type of enemy - affects behavior")]
    public EnemyType enemyType = EnemyType.MushroomGuardian;
    
    [Header("Combat Settings")]
    [Tooltip("Range at which enemy detects player")]
    public float detectionRange = 15f;
    
    [Tooltip("Range at which enemy attacks")]
    public float attackRange = 2f;
    
    [Tooltip("Damage dealt per attack")]
    public int attackDamage = 15;
    
    [Tooltip("Time between attacks")]
    public float attackCooldown = 2f;
    
    [Header("Patrol Settings")]
    [Tooltip("Patrol points for the enemy")]
    public Transform[] patrolPoints;
    
    [Tooltip("Wait time at each patrol point")]
    public float patrolWaitTime = 2f;
    
    [Header("Behavior Settings")]
    [Tooltip("Return to patrol after losing sight of player")]
    public bool returnToPatrol = true;
    
    [Tooltip("Call nearby allies when attacked")]
    public bool callForHelp = true;
    
    [Tooltip("Help call radius")]
    public float helpCallRadius = 20f;
    
    private NavMeshAgent agent;
    private Health health;
    private Transform player;
    private EnemyState currentState = EnemyState.Patrol;
    private int currentPatrolIndex = 0;
    private float lastAttackTime;
    private float patrolWaitTimer;
    private Vector3 lastKnownPlayerPosition;
    
    public enum EnemyType
    {
        MushroomGuardian,   // Guards mushroom mountains
        WaterfallSentinel,  // Guards waterfalls
        LostSoul,           // Wandering aggressive enemy
        ForestWarden        // Patrol-focused protector
    }
    
    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Retreat,
        Dead
    }
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        
        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        
        // Start with patrol if points exist, otherwise idle
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            currentState = EnemyState.Patrol;
            MoveToNextPatrolPoint();
        }
        else
        {
            currentState = EnemyState.Idle;
        }
    }
    
    void Update()
    {
        if (health.currentHP <= 0)
        {
            currentState = EnemyState.Dead;
            agent.isStopped = true;
            return;
        }
        
        float distanceToPlayer = player != null ? Vector3.Distance(transform.position, player.position) : float.MaxValue;
        
        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdleState(distanceToPlayer);
                break;
                
            case EnemyState.Patrol:
                HandlePatrolState(distanceToPlayer);
                break;
                
            case EnemyState.Chase:
                HandleChaseState(distanceToPlayer);
                break;
                
            case EnemyState.Attack:
                HandleAttackState(distanceToPlayer);
                break;
                
            case EnemyState.Retreat:
                HandleRetreatState(distanceToPlayer);
                break;
        }
    }
    
    void HandleIdleState(float distanceToPlayer)
    {
        if (distanceToPlayer <= detectionRange)
        {
            currentState = EnemyState.Chase;
        }
    }
    
    void HandlePatrolState(float distanceToPlayer)
    {
        if (distanceToPlayer <= detectionRange)
        {
            currentState = EnemyState.Chase;
            lastKnownPlayerPosition = player.position;
            return;
        }
        
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            patrolWaitTimer += Time.deltaTime;
            if (patrolWaitTimer >= patrolWaitTime)
            {
                MoveToNextPatrolPoint();
                patrolWaitTimer = 0f;
            }
        }
    }
    
    void HandleChaseState(float distanceToPlayer)
    {
        if (player == null)
        {
            currentState = returnToPatrol ? EnemyState.Patrol : EnemyState.Idle;
            return;
        }
        
        if (distanceToPlayer > detectionRange * 1.5f)
        {
            // Lost sight of player
            currentState = returnToPatrol ? EnemyState.Patrol : EnemyState.Idle;
            return;
        }
        
        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attack;
            agent.isStopped = true;
            return;
        }
        
        agent.isStopped = false;
        agent.SetDestination(player.position);
        lastKnownPlayerPosition = player.position;
    }
    
    void HandleAttackState(float distanceToPlayer)
    {
        if (player == null)
        {
            currentState = EnemyState.Patrol;
            return;
        }
        
        if (distanceToPlayer > attackRange * 1.2f)
        {
            currentState = EnemyState.Chase;
            agent.isStopped = false;
            return;
        }
        
        // Face the player
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        
        // Attack if cooldown is ready
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }
    }
    
    void HandleRetreatState(float distanceToPlayer)
    {
        // Simple retreat: move away from player
        if (player != null && distanceToPlayer < detectionRange)
        {
            Vector3 retreatDirection = (transform.position - player.position).normalized;
            Vector3 retreatPosition = transform.position + retreatDirection * 10f;
            agent.SetDestination(retreatPosition);
        }
        else
        {
            currentState = returnToPatrol ? EnemyState.Patrol : EnemyState.Idle;
        }
    }
    
    void PerformAttack()
    {
        if (player == null) return;
        
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log($"{gameObject.name} attacked player for {attackDamage} damage!");
        }
        
        // Call for help if enabled
        if (callForHelp)
        {
            CallNearbyAllies();
        }
    }
    
    void MoveToNextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;
        
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }
    
    void CallNearbyAllies()
    {
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, helpCallRadius);
        foreach (Collider col in nearbyColliders)
        {
            EnemyAI ally = col.GetComponent<EnemyAI>();
            if (ally != null && ally != this && ally.currentState != EnemyState.Chase && ally.currentState != EnemyState.Attack)
            {
                ally.AlertToPlayer(player);
            }
        }
    }
    
    public void AlertToPlayer(Transform playerTransform)
    {
        player = playerTransform;
        currentState = EnemyState.Chase;
        lastKnownPlayerPosition = player.position;
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        // Draw help call radius
        if (callForHelp)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, helpCallRadius);
        }
        
        // Draw patrol path
        if (patrolPoints != null && patrolPoints.Length > 1)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] != null)
                {
                    Vector3 currentPoint = patrolPoints[i].position;
                    Vector3 nextPoint = patrolPoints[(i + 1) % patrolPoints.Length].position;
                    Gizmos.DrawLine(currentPoint, nextPoint);
                    Gizmos.DrawWireSphere(currentPoint, 0.5f);
                }
            }
        }
    }
}
