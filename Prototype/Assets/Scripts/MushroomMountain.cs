using UnityEngine;

/// <summary>
/// Procedural mushroom mountain generator that creates mushroom-like terrain structures
/// Place this on empty GameObjects to spawn procedural mushroom mountains in the scene
/// </summary>
public class MushroomMountain : MonoBehaviour
{
    [Header("Mushroom Mountain Settings")]
    [Tooltip("Height of the mushroom mountain")]
    public float height = 20f;
    
    [Tooltip("Radius of the mushroom cap")]
    public float capRadius = 15f;
    
    [Tooltip("Radius of the stem")]
    public float stemRadius = 5f;
    
    [Tooltip("Color of the mushroom cap")]
    public Color capColor = new Color(0.8f, 0.2f, 0.2f); // Red cap
    
    [Tooltip("Color of the mushroom stem")]
    public Color stemColor = new Color(0.9f, 0.9f, 0.8f); // Beige stem
    
    [Tooltip("Add spots to the mushroom cap")]
    public bool hasSpots = true;
    
    [Tooltip("Number of spots on the cap")]
    public int spotCount = 8;
    
    [Header("Gameplay Elements")]
    [Tooltip("Can players climb this mushroom?")]
    public bool isClimbable = true;
    
    [Tooltip("Health restoration amount when near this mushroom")]
    public int healingAmount = 10;
    
    [Tooltip("Healing radius around the mushroom")]
    public float healingRadius = 5f;
    
    private GameObject capObject;
    private GameObject stemObject;
    private float lastHealTime;
    
    void Start()
    {
        GenerateMushroomMountain();
        lastHealTime = Time.time;
    }
    
    void GenerateMushroomMountain()
    {
        // Create stem
        stemObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        stemObject.transform.SetParent(transform);
        stemObject.transform.localPosition = Vector3.up * (height * 0.3f);
        stemObject.transform.localScale = new Vector3(stemRadius, height * 0.3f, stemRadius);
        stemObject.name = "MushroomStem";
        
        Renderer stemRenderer = stemObject.GetComponent<Renderer>();
        if (stemRenderer != null)
        {
            stemRenderer.material = new Material(Shader.Find("Standard"));
            stemRenderer.material.color = stemColor;
        }
        
        // Create cap
        capObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        capObject.transform.SetParent(transform);
        capObject.transform.localPosition = Vector3.up * (height * 0.6f + height * 0.3f);
        capObject.transform.localScale = new Vector3(capRadius, height * 0.4f, capRadius);
        capObject.name = "MushroomCap";
        
        Renderer capRenderer = capObject.GetComponent<Renderer>();
        if (capRenderer != null)
        {
            capRenderer.material = new Material(Shader.Find("Standard"));
            capRenderer.material.color = capColor;
        }
        
        // Add spots if enabled
        if (hasSpots)
        {
            CreateSpots();
        }
    }
    
    void CreateSpots()
    {
        for (int i = 0; i < spotCount; i++)
        {
            GameObject spot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spot.transform.SetParent(capObject.transform);
            
            // Random position on cap surface
            float angle = (360f / spotCount) * i;
            float randomRadius = Random.Range(0.3f, 0.8f);
            Vector3 localPos = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * randomRadius,
                Random.Range(-0.3f, 0.2f),
                Mathf.Sin(angle * Mathf.Deg2Rad) * randomRadius
            );
            spot.transform.localPosition = localPos;
            spot.transform.localScale = Vector3.one * Random.Range(0.05f, 0.15f);
            spot.name = "Spot" + i;
            
            Renderer spotRenderer = spot.GetComponent<Renderer>();
            if (spotRenderer != null)
            {
                spotRenderer.material = new Material(Shader.Find("Standard"));
                spotRenderer.material.color = Color.white;
            }
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw healing radius
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawWireSphere(transform.position, healingRadius);
    }
    
    void Update()
    {
        // Heal nearby players once per second (frame-rate independent)
        if (Time.time - lastHealTime >= 1f)
        {
            lastHealTime = Time.time;
            
            Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, healingRadius);
            foreach (Collider col in nearbyColliders)
            {
                if (col.CompareTag("Player"))
                {
                    Health health = col.GetComponent<Health>();
                    if (health != null && health.currentHP < health.maxHP)
                    {
                        health.Heal(1);
                    }
                }
            }
        }
    }
}
