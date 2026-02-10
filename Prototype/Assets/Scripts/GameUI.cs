using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles all UI elements for the Island of the Lost Time game
/// including health bars, story text, and game information
/// </summary>
public class GameUI : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Reference to the player")]
    public PlayerController player;
    
    [Tooltip("Story manager reference")]
    public StoryManager storyManager;
    
    [Header("UI Display Settings")]
    [Tooltip("Show debug info on screen")]
    public bool showDebugInfo = true;
    
    [Tooltip("Show controls info")]
    public bool showControls = true;
    
    [Tooltip("Font size for UI text")]
    public int fontSize = 16;
    
    private GUIStyle titleStyle;
    private GUIStyle normalStyle;
    private GUIStyle healthStyle;
    private Health playerHealth;
    
    void Start()
    {
        InitializeStyles();
        
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.GetComponent<PlayerController>();
                playerHealth = playerObj.GetComponent<Health>();
            }
        }
        else
        {
            playerHealth = player.GetComponent<Health>();
        }
        
        if (storyManager == null)
        {
            storyManager = FindObjectOfType<StoryManager>();
        }
    }
    
    void InitializeStyles()
    {
        titleStyle = new GUIStyle();
        titleStyle.fontSize = fontSize + 8;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.normal.textColor = Color.white;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        
        normalStyle = new GUIStyle();
        normalStyle.fontSize = fontSize;
        normalStyle.normal.textColor = Color.white;
        normalStyle.padding = new RectOffset(10, 10, 5, 5);
        
        healthStyle = new GUIStyle();
        healthStyle.fontSize = fontSize + 2;
        healthStyle.fontStyle = FontStyle.Bold;
        healthStyle.normal.textColor = Color.green;
        healthStyle.padding = new RectOffset(10, 10, 5, 5);
    }
    
    void OnGUI()
    {
        if (titleStyle == null) InitializeStyles();
        
        // Title
        GUI.Label(new Rect(Screen.width / 2 - 200, 10, 400, 40), 
            "ISLAND OF THE LOST TIME", titleStyle);
        
        // Health Bar
        DrawHealthBar();
        
        // Controls
        if (showControls)
        {
            DrawControls();
        }
        
        // Story Info
        DrawStoryInfo();
        
        // Debug Info
        if (showDebugInfo)
        {
            DrawDebugInfo();
        }
    }
    
    void DrawHealthBar()
    {
        if (playerHealth == null) return;
        
        float healthPercent = (float)playerHealth.currentHP / playerHealth.maxHP;
        
        // Background
        GUI.color = Color.black;
        GUI.DrawTexture(new Rect(10, 50, 204, 24), Texture2D.whiteTexture);
        
        // Health bar
        if (healthPercent > 0.5f)
            GUI.color = Color.green;
        else if (healthPercent > 0.25f)
            GUI.color = Color.yellow;
        else
            GUI.color = Color.red;
            
        GUI.DrawTexture(new Rect(12, 52, 200 * healthPercent, 20), Texture2D.whiteTexture);
        
        // Text
        GUI.color = Color.white;
        string healthText = $"HP: {playerHealth.currentHP} / {playerHealth.maxHP}";
        GUI.Label(new Rect(10, 50, 200, 30), healthText, healthStyle);
    }
    
    void DrawControls()
    {
        GUI.color = Color.white;
        float yPos = 80;
        float lineHeight = fontSize + 5;
        
        GUI.Label(new Rect(10, yPos, 300, lineHeight), "=== CONTROLS ===", normalStyle);
        yPos += lineHeight;
        
        GUI.Label(new Rect(10, yPos, 300, lineHeight), "WASD - Move", normalStyle);
        yPos += lineHeight;
        
        GUI.Label(new Rect(10, yPos, 300, lineHeight), "Mouse - Look Around", normalStyle);
        yPos += lineHeight;
        
        GUI.Label(new Rect(10, yPos, 300, lineHeight), "E - Mount/Dismount Horse", normalStyle);
        yPos += lineHeight;
        
        GUI.Label(new Rect(10, yPos, 300, lineHeight), "Shift - Sprint/Gallop", normalStyle);
        yPos += lineHeight;
        
        GUI.Label(new Rect(10, yPos, 300, lineHeight), "Left Click - Attack", normalStyle);
        yPos += lineHeight;
        
        GUI.Label(new Rect(10, yPos, 300, lineHeight), "H - Rally Squad", normalStyle);
        yPos += lineHeight;
        
        GUI.Label(new Rect(10, yPos, 300, lineHeight), "Tab - Story Progress", normalStyle);
        yPos += lineHeight;
        
        GUI.Label(new Rect(10, yPos, 300, lineHeight), "F1 - Show Hint", normalStyle);
    }
    
    void DrawStoryInfo()
    {
        if (storyManager == null) return;
        
        GUI.color = Color.white;
        float yPos = Screen.height - 100;
        
        GUI.Label(new Rect(10, yPos, 500, 30), 
            $"Chapter {storyManager.currentChapter} / {storyManager.totalChapters}", 
            normalStyle);
    }
    
    void DrawDebugInfo()
    {
        GUI.color = Color.white;
        float xPos = Screen.width - 250;
        float yPos = 50;
        float lineHeight = fontSize + 5;
        
        GUI.Label(new Rect(xPos, yPos, 240, lineHeight), "=== DEBUG INFO ===", normalStyle);
        yPos += lineHeight;
        
        if (player != null)
        {
            GUI.Label(new Rect(xPos, yPos, 240, lineHeight), 
                $"Mounted: {player.isMounted}", normalStyle);
            yPos += lineHeight;
            
            GUI.Label(new Rect(xPos, yPos, 240, lineHeight), 
                $"Position: {player.transform.position.ToString("F1")}", normalStyle);
            yPos += lineHeight;
        }
        
        GUI.Label(new Rect(xPos, yPos, 240, lineHeight), 
            $"FPS: {(int)(1f / Time.deltaTime)}", normalStyle);
        yPos += lineHeight;
        
        int enemyCount = FindObjectsOfType<EnemyAI>().Length;
        GUI.Label(new Rect(xPos, yPos, 240, lineHeight), 
            $"Enemies: {enemyCount}", normalStyle);
    }
}
