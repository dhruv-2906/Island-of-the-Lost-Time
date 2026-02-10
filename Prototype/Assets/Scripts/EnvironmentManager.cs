using UnityEngine;

/// <summary>
/// Manages the 3D adventure game environment including mushroom mountains,
/// waterfalls, and terrain generation for Island of the Lost Time
/// </summary>
public class EnvironmentManager : MonoBehaviour
{
    [Header("Environment Generation")]
    [Tooltip("Generate environment on start")]
    public bool autoGenerate = true;
    
    [Tooltip("Number of mushroom mountains to generate")]
    public int mushroomMountainCount = 5;
    
    [Tooltip("Number of waterfalls to generate")]
    public int waterfallCount = 3;
    
    [Tooltip("Area size for generation")]
    public float generationRadius = 50f;
    
    [Header("Prefabs (Optional)")]
    [Tooltip("Custom mushroom mountain prefab")]
    public GameObject mushroomMountainPrefab;
    
    [Tooltip("Custom waterfall prefab")]
    public GameObject waterfallPrefab;
    
    [Header("Visual Settings")]
    [Tooltip("Enable fog for atmosphere")]
    public bool enableFog = true;
    
    [Tooltip("Fog color")]
    public Color fogColor = new Color(0.7f, 0.8f, 0.9f);
    
    [Tooltip("Fog density")]
    public float fogDensity = 0.01f;
    
    [Header("Lighting")]
    [Tooltip("Ambient light color")]
    public Color ambientColor = new Color(0.5f, 0.6f, 0.7f);
    
    void Start()
    {
        if (autoGenerate)
        {
            GenerateEnvironment();
        }
        
        SetupAtmosphere();
    }
    
    void GenerateEnvironment()
    {
        // Generate mushroom mountains
        for (int i = 0; i < mushroomMountainCount; i++)
        {
            GenerateMushroomMountain(i);
        }
        
        // Generate waterfalls
        for (int i = 0; i < waterfallCount; i++)
        {
            GenerateWaterfall(i);
        }
        
        Debug.Log($"Environment generated: {mushroomMountainCount} mushroom mountains and {waterfallCount} waterfalls");
    }
    
    void GenerateMushroomMountain(int index)
    {
        // Random position in generation area
        Vector2 randomCircle = Random.insideUnitCircle * generationRadius;
        Vector3 position = new Vector3(randomCircle.x, 0, randomCircle.y);
        
        GameObject mushroom;
        if (mushroomMountainPrefab != null)
        {
            mushroom = Instantiate(mushroomMountainPrefab, position, Quaternion.identity, transform);
        }
        else
        {
            // Create procedural mushroom mountain
            mushroom = new GameObject($"MushroomMountain_{index}");
            mushroom.transform.SetParent(transform);
            mushroom.transform.position = position;
            
            MushroomMountain mushroomScript = mushroom.AddComponent<MushroomMountain>();
            
            // Randomize properties
            mushroomScript.height = Random.Range(15f, 30f);
            mushroomScript.capRadius = Random.Range(10f, 20f);
            mushroomScript.stemRadius = Random.Range(4f, 7f);
            
            // Random colors for variety
            Color[] capColors = new Color[]
            {
                new Color(0.8f, 0.2f, 0.2f), // Red
                new Color(0.9f, 0.5f, 0.2f), // Orange
                new Color(0.6f, 0.3f, 0.8f), // Purple
                new Color(0.3f, 0.7f, 0.9f)  // Blue
            };
            mushroomScript.capColor = capColors[Random.Range(0, capColors.Length)];
            mushroomScript.hasSpots = Random.value > 0.3f;
        }
        
        mushroom.name = $"MushroomMountain_{index}";
    }
    
    void GenerateWaterfall(int index)
    {
        // Position waterfalls at edges or elevated areas
        Vector2 randomCircle = Random.insideUnitCircle.normalized * generationRadius * 0.8f;
        Vector3 position = new Vector3(randomCircle.x, Random.Range(5f, 15f), randomCircle.y);
        
        GameObject waterfall;
        if (waterfallPrefab != null)
        {
            waterfall = Instantiate(waterfallPrefab, position, Quaternion.identity, transform);
        }
        else
        {
            // Create procedural waterfall
            waterfall = new GameObject($"Waterfall_{index}");
            waterfall.transform.SetParent(transform);
            waterfall.transform.position = position;
            
            WaterfallEffect waterfallScript = waterfall.AddComponent<WaterfallEffect>();
            waterfallScript.waterfallHeight = Random.Range(10f, 20f);
            waterfallScript.waterfallWidth = Random.Range(3f, 7f);
            waterfallScript.createMist = true;
            waterfallScript.createSplash = true;
        }
        
        waterfall.name = $"Waterfall_{index}";
    }
    
    void SetupAtmosphere()
    {
        // Setup fog
        if (enableFog)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogDensity;
            RenderSettings.fogMode = FogMode.Exponential;
        }
        
        // Setup ambient lighting
        RenderSettings.ambientLight = ambientColor;
    }
    
    public void RegenerateEnvironment()
    {
        // Clear existing environment
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        // Generate new environment
        GenerateEnvironment();
    }
    
    void OnDrawGizmosSelected()
    {
        // Visualize generation area
        Gizmos.color = new Color(0, 1, 0, 0.2f);
        Gizmos.DrawWireSphere(transform.position, generationRadius);
    }
}
