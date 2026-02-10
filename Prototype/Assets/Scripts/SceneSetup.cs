using UnityEngine;

/// <summary>
/// Automated scene setup helper for Island of the Lost Time
/// This script helps quickly set up a playable scene with all necessary components
/// </summary>
public class SceneSetup : MonoBehaviour
{
    [Header("Auto Setup Options")]
    [Tooltip("Automatically setup scene on start")]
    public bool autoSetup = true;
    
    [Tooltip("Create player automatically")]
    public bool createPlayer = true;
    
    [Tooltip("Create enemies automatically")]
    public bool createEnemies = true;
    
    [Tooltip("Number of enemies to spawn")]
    public int enemyCount = 5;
    
    [Tooltip("Create camera automatically")]
    public bool createCamera = true;
    
    [Tooltip("Create ground plane")]
    public bool createGround = true;
    
    [Tooltip("Setup lighting")]
    public bool setupLighting = true;
    
    void Start()
    {
        if (autoSetup)
        {
            SetupScene();
        }
    }
    
    [ContextMenu("Setup Scene")]
    public void SetupScene()
    {
        Debug.Log("Starting automated scene setup...");
        
        // Setup managers first
        SetupGameManagers();
        
        // Create ground
        if (createGround)
        {
            CreateGround();
        }
        
        // Create player
        if (createPlayer)
        {
            CreatePlayer();
        }
        
        // Create camera
        if (createCamera)
        {
            CreateGameCamera();
        }
        
        // Create enemies
        if (createEnemies)
        {
            CreateEnemies();
        }
        
        // Setup lighting
        if (setupLighting)
        {
            SetupSceneLighting();
        }
        
        Debug.Log("Scene setup complete!");
    }
    
    void SetupGameManagers()
    {
        // Game Manager
        if (FindObjectOfType<GameManager>() == null)
        {
            GameObject gmObj = new GameObject("GameManager");
            gmObj.AddComponent<GameManager>();
            Debug.Log("Created GameManager");
        }
        
        // Story Manager
        if (FindObjectOfType<StoryManager>() == null)
        {
            GameObject smObj = new GameObject("StoryManager");
            smObj.AddComponent<StoryManager>();
            Debug.Log("Created StoryManager");
        }
        
        // Environment Manager
        if (FindObjectOfType<EnvironmentManager>() == null)
        {
            GameObject emObj = new GameObject("EnvironmentManager");
            emObj.AddComponent<EnvironmentManager>();
            Debug.Log("Created EnvironmentManager");
        }
        
        // UI Manager
        if (FindObjectOfType<GameUI>() == null)
        {
            GameObject uiObj = new GameObject("GameUI");
            uiObj.AddComponent<GameUI>();
            Debug.Log("Created GameUI");
        }
    }
    
    void CreateGround()
    {
        if (GameObject.Find("Ground") != null)
        {
            Debug.Log("Ground already exists");
            return;
        }
        
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = new Vector3(20, 1, 20); // 200x200 units
        
        // Make it look like grass
        Renderer renderer = ground.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = new Material(Shader.Find("Standard"));
            renderer.material.color = new Color(0.3f, 0.6f, 0.3f); // Green
        }
        
        Debug.Log("Created ground plane");
    }
    
    void CreatePlayer()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            Debug.Log("Player already exists");
            return;
        }
        
        GameObject player = new GameObject("Player");
        player.tag = "Player";
        player.transform.position = new Vector3(0, 1, 0);
        
        // Add character controller
        CharacterController cc = player.AddComponent<CharacterController>();
        cc.radius = 0.5f;
        cc.height = 2f;
        cc.center = new Vector3(0, 1, 0);
        
        // Add player controller
        player.AddComponent<PlayerController>();
        
        // Add health
        Health health = player.AddComponent<Health>();
        health.maxHP = 100;
        
        // Add melee attack
        MeleeAttack melee = player.AddComponent<MeleeAttack>();
        melee.range = 2f;
        melee.damage = 25;
        
        // Visual representation
        GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        visual.transform.SetParent(player.transform);
        visual.transform.localPosition = new Vector3(0, 1, 0);
        Destroy(visual.GetComponent<Collider>()); // Remove collider, CharacterController handles it
        
        Renderer renderer = visual.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = new Material(Shader.Find("Standard"));
            renderer.material.color = new Color(0.2f, 0.6f, 1f); // Blue
        }
        
        Debug.Log("Created player at " + player.transform.position);
    }
    
    void CreateGameCamera()
    {
        Camera existingCamera = Camera.main;
        if (existingCamera != null && existingCamera.GetComponent<CameraOrbit>() != null)
        {
            Debug.Log("Camera with CameraOrbit already exists");
            return;
        }
        
        GameObject cameraObj;
        if (existingCamera != null)
        {
            cameraObj = existingCamera.gameObject;
        }
        else
        {
            cameraObj = new GameObject("Main Camera");
            cameraObj.tag = "MainCamera";
            cameraObj.AddComponent<Camera>();
        }
        
        // Position camera
        cameraObj.transform.position = new Vector3(0, 5, -10);
        cameraObj.transform.rotation = Quaternion.Euler(20, 0, 0);
        
        // Add camera orbit
        CameraOrbit orbit = cameraObj.GetComponent<CameraOrbit>();
        if (orbit == null)
        {
            orbit = cameraObj.AddComponent<CameraOrbit>();
        }
        
        // Set target to player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            orbit.target = player.transform;
        }
        
        Debug.Log("Created/configured camera");
    }
    
    void CreateEnemies()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Cannot create enemies without a player");
            return;
        }
        
        for (int i = 0; i < enemyCount; i++)
        {
            // Random position around player
            Vector2 randomCircle = Random.insideUnitCircle * 20f;
            Vector3 position = new Vector3(randomCircle.x, 0.5f, randomCircle.y) + Vector3.right * 10f;
            
            GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = $"Enemy_{i}";
            enemy.tag = "Enemy";
            enemy.transform.position = position;
            
            // Add NavMeshAgent
            UnityEngine.AI.NavMeshAgent agent = enemy.AddComponent<UnityEngine.AI.NavMeshAgent>();
            agent.radius = 0.5f;
            agent.height = 2f;
            agent.speed = 3.5f;
            
            // Add AI
            EnemyAI ai = enemy.AddComponent<EnemyAI>();
            ai.detectionRange = 15f;
            ai.attackRange = 2f;
            ai.attackDamage = 15;
            
            // Add health
            Health health = enemy.AddComponent<Health>();
            health.maxHP = 50;
            
            // Color
            Renderer renderer = enemy.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = new Material(Shader.Find("Standard"));
                renderer.material.color = new Color(0.8f, 0.2f, 0.2f); // Red
            }
            
            Debug.Log($"Created enemy {i} at {position}");
        }
    }
    
    void SetupSceneLighting()
    {
        // Create directional light if none exists
        Light[] lights = FindObjectsOfType<Light>();
        bool hasDirectionalLight = false;
        
        foreach (Light light in lights)
        {
            if (light.type == LightType.Directional)
            {
                hasDirectionalLight = true;
                break;
            }
        }
        
        if (!hasDirectionalLight)
        {
            GameObject lightObj = new GameObject("Directional Light");
            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = new Color(1f, 0.95f, 0.8f);
            light.intensity = 1f;
            lightObj.transform.rotation = Quaternion.Euler(50, -30, 0);
            Debug.Log("Created directional light");
        }
        
        // Setup ambient light
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.ambientSkyColor = new Color(0.5f, 0.6f, 0.7f);
        RenderSettings.ambientEquatorColor = new Color(0.4f, 0.4f, 0.4f);
        RenderSettings.ambientGroundColor = new Color(0.2f, 0.3f, 0.2f);
    }
    
    [ContextMenu("Create NavMesh")]
    public void CreateNavMeshInfo()
    {
        Debug.Log(@"
To enable enemy AI navigation:
1. Window → AI → Navigation
2. Select the Ground plane
3. In Navigation window, check 'Navigation Static'
4. Click 'Bake' button
5. Adjust settings if needed:
   - Agent Radius: 0.5
   - Agent Height: 2
   - Max Slope: 45
");
    }
}
