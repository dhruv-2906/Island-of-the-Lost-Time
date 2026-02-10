using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Squad Management")]
    public Transform[] squadMembers;

    [Header("Game Systems")]
    public StoryManager storyManager;
    public EnvironmentManager environmentManager;
    
    private static GameManager instance;
    
    public static GameManager Instance
    {
        get { return instance; }
    }
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Initialize game systems
        if (storyManager == null)
        {
            storyManager = FindObjectOfType<StoryManager>();
            if (storyManager == null)
            {
                GameObject storyObj = new GameObject("StoryManager");
                storyManager = storyObj.AddComponent<StoryManager>();
            }
        }
        
        if (environmentManager == null)
        {
            environmentManager = FindObjectOfType<EnvironmentManager>();
            if (environmentManager == null)
            {
                GameObject envObj = new GameObject("EnvironmentManager");
                environmentManager = envObj.AddComponent<EnvironmentManager>();
            }
        }
    }

    public void RallySquad(Vector3 playerPos)
    {
        Debug.Log("Rally horn sounded! Squad moving to player position.");
        
        foreach (var m in squadMembers)
        {
            if (m == null) continue;
            
            var ai = m.GetComponent<SquadAI>();
            if (ai != null)
                ai.Rally(FindObjectOfType<PlayerController>().transform);
        }
    }
    
    public void OnEnemyDefeated(GameObject enemy)
    {
        if (storyManager != null)
        {
            storyManager.OnEnemyDefeated(enemy);
        }
    }
}
