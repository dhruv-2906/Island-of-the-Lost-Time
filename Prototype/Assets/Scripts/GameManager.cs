using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform[] squadMembers;

    public void RallySquad(Vector3 playerPos)
    {
        foreach (var m in squadMembers)
        {
            var ai = m.GetComponent<SquadAI>();
            if (ai != null)
                ai.Rally(FindObjectOfType<PlayerController>().transform);
        }
    }
}
