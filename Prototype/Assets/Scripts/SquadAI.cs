using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SquadAI : MonoBehaviour
{
    public Transform leader;
    public float followDistance = 3f;

    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (leader != null)
        {
            Vector3 target = leader.position - leader.forward * followDistance;
            agent.SetDestination(target);
        }
    }

    public void SetLeader(Transform t)
    {
        leader = t;
    }

    public void Rally(Transform player)
    {
        leader = player;
    }
}
