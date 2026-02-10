using UnityEngine;

/// <summary>
/// Creates a realistic waterfall effect using particle system and water shader
/// Attach to a GameObject to create a waterfall at that position
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class WaterfallEffect : MonoBehaviour
{
    [Header("Waterfall Settings")]
    [Tooltip("Height of the waterfall")]
    public float waterfallHeight = 15f;
    
    [Tooltip("Width of the waterfall")]
    public float waterfallWidth = 5f;
    
    [Tooltip("Flow rate (particles per second)")]
    public float flowRate = 500f;
    
    [Tooltip("Water color")]
    public Color waterColor = new Color(0.5f, 0.7f, 1f, 0.6f);
    
    [Tooltip("Create mist effect at the bottom")]
    public bool createMist = true;
    
    [Tooltip("Create splash effect at the bottom")]
    public bool createSplash = true;
    
    [Header("Audio")]
    [Tooltip("Waterfall sound effect (optional)")]
    public AudioClip waterfallSound;
    
    private ParticleSystem waterParticles;
    private ParticleSystem mistParticles;
    private GameObject waterPool;
    private AudioSource audioSource;
    
    void Start()
    {
        SetupWaterfall();
    }
    
    void SetupWaterfall()
    {
        // Setup main water particle system
        waterParticles = GetComponent<ParticleSystem>();
        var main = waterParticles.main;
        main.startColor = waterColor;
        main.startSpeed = 10f;
        main.startSize = 0.5f;
        main.startLifetime = waterfallHeight / 10f;
        main.maxParticles = (int)flowRate * 5;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        
        var emission = waterParticles.emission;
        emission.rateOverTime = flowRate;
        
        var shape = waterParticles.shape;
        shape.shapeType = ParticleSystemShapeType.Rectangle;
        shape.scale = new Vector3(waterfallWidth, 0.1f, 0.1f);
        
        // Add gravity
        var forceOverLifetime = waterParticles.forceOverLifetime;
        forceOverLifetime.enabled = true;
        forceOverLifetime.y = -20f;
        
        // Create water pool at the bottom
        CreateWaterPool();
        
        // Create mist effect
        if (createMist)
        {
            CreateMistEffect();
        }
        
        // Setup audio
        if (waterfallSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = waterfallSound;
            audioSource.loop = true;
            audioSource.spatialBlend = 1f; // 3D sound
            audioSource.maxDistance = 50f;
            audioSource.Play();
        }
    }
    
    void CreateWaterPool()
    {
        waterPool = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        waterPool.transform.SetParent(transform);
        waterPool.transform.localPosition = Vector3.down * waterfallHeight;
        waterPool.transform.localScale = new Vector3(waterfallWidth * 2, 0.1f, waterfallWidth * 2);
        waterPool.name = "WaterPool";
        
        Renderer poolRenderer = waterPool.GetComponent<Renderer>();
        if (poolRenderer != null)
        {
            Material waterMaterial = new Material(Shader.Find("Standard"));
            waterMaterial.color = new Color(waterColor.r, waterColor.g, waterColor.b, 0.5f);
            waterMaterial.SetFloat("_Metallic", 0.5f);
            waterMaterial.SetFloat("_Glossiness", 0.9f);
            poolRenderer.material = waterMaterial;
        }
        
        // Make water pool trigger for effects
        Collider poolCollider = waterPool.GetComponent<Collider>();
        if (poolCollider != null)
        {
            poolCollider.isTrigger = true;
        }
    }
    
    void CreateMistEffect()
    {
        GameObject mistObject = new GameObject("WaterfallMist");
        mistObject.transform.SetParent(transform);
        mistObject.transform.localPosition = Vector3.down * waterfallHeight;
        
        mistParticles = mistObject.AddComponent<ParticleSystem>();
        var main = mistParticles.main;
        main.startColor = new Color(1f, 1f, 1f, 0.3f);
        main.startSpeed = 2f;
        main.startSize = 2f;
        main.startLifetime = 3f;
        main.maxParticles = 100;
        
        var emission = mistParticles.emission;
        emission.rateOverTime = 50f;
        
        var shape = mistParticles.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = waterfallWidth;
    }
    
    void OnDrawGizmosSelected()
    {
        // Visualize waterfall area
        Gizmos.color = new Color(0.5f, 0.7f, 1f, 0.5f);
        Gizmos.DrawWireCube(transform.position - Vector3.up * (waterfallHeight * 0.5f), 
            new Vector3(waterfallWidth, waterfallHeight, waterfallWidth));
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Add water splash effect when something enters the pool
        if (createSplash && other.CompareTag("Player"))
        {
            CreateSplash(other.transform.position);
        }
    }
    
    void CreateSplash(Vector3 position)
    {
        // Simple splash effect
        GameObject splash = new GameObject("Splash");
        splash.transform.position = position;
        ParticleSystem splashPS = splash.AddComponent<ParticleSystem>();
        
        var main = splashPS.main;
        main.startColor = waterColor;
        main.startSpeed = 5f;
        main.startSize = 0.3f;
        main.startLifetime = 0.5f;
        main.maxParticles = 20;
        
        var emission = splashPS.emission;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0, 20) });
        
        Destroy(splash, 2f);
    }
}
