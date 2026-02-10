using UnityEngine;

/// <summary>
/// Story progression system for the Island of the Lost Time adventure
/// Manages quest stages, story triggers, and narrative elements
/// </summary>
public class StoryManager : MonoBehaviour
{
    [Header("Story Settings")]
    [Tooltip("Current story chapter")]
    public int currentChapter = 1;
    
    [Tooltip("Total chapters in the story")]
    public int totalChapters = 5;
    
    [Header("Story Text")]
    [TextArea(3, 10)]
    public string introText = @"Welcome to the Island of the Lost Time...

You awaken in a mysterious land of giant mushroom mountains and cascading waterfalls. 
The ancient guardians of this realm have been corrupted by a dark force.

Your mission: Restore balance to the island by defeating the corrupted guardians 
and discovering the source of the corruption.

Press H to rally your squad. Use WASD to move, E to mount/dismount your horse, 
and left-click to attack.";
    
    [TextArea(3, 5)]
    public string[] chapterTitles = new string[]
    {
        "Chapter 1: Awakening in the Mushroom Forest",
        "Chapter 2: The Waterfall Sentinels",
        "Chapter 3: The Lost Souls",
        "Chapter 4: The Corruption Source",
        "Chapter 5: The Final Guardian"
    };
    
    [TextArea(3, 5)]
    public string[] chapterDescriptions = new string[]
    {
        "Explore the mushroom forest and defeat the corrupted mushroom guardians.",
        "Find the waterfalls and cleanse them from the waterfall sentinels.",
        "Help the lost souls find peace by defeating their tormentors.",
        "Journey to the heart of the island to discover the source of corruption.",
        "Face the final guardian and restore balance to the Island of the Lost Time."
    };
    
    private int enemiesDefeated = 0;
    private int[] enemiesPerChapter = new int[] { 3, 5, 7, 10, 1 };
    
    void Start()
    {
        ShowStoryIntro();
    }
    
    void ShowStoryIntro()
    {
        Debug.Log("=== ISLAND OF THE LOST TIME ===");
        Debug.Log(introText);
        Debug.Log("\n" + GetCurrentChapterText());
    }
    
    public string GetCurrentChapterText()
    {
        if (currentChapter > 0 && currentChapter <= chapterTitles.Length)
        {
            return $"\n{chapterTitles[currentChapter - 1]}\n{chapterDescriptions[currentChapter - 1]}";
        }
        return "";
    }
    
    public void OnEnemyDefeated(GameObject enemy)
    {
        enemiesDefeated++;
        Debug.Log($"Enemy defeated! Total: {enemiesDefeated}");
        
        // Check if chapter is complete
        if (currentChapter <= enemiesPerChapter.Length && 
            enemiesDefeated >= GetEnemiesRequiredForCurrentChapter())
        {
            CompleteChapter();
        }
    }
    
    int GetEnemiesRequiredForCurrentChapter()
    {
        int total = 0;
        for (int i = 0; i < currentChapter && i < enemiesPerChapter.Length; i++)
        {
            total += enemiesPerChapter[i];
        }
        return total;
    }
    
    void CompleteChapter()
    {
        Debug.Log($"\n=== CHAPTER {currentChapter} COMPLETE! ===\n");
        
        currentChapter++;
        
        if (currentChapter <= totalChapters)
        {
            Debug.Log(GetCurrentChapterText());
        }
        else
        {
            ShowVictory();
        }
    }
    
    void ShowVictory()
    {
        Debug.Log(@"
=================================================
           VICTORY!
=================================================

You have restored balance to the Island of the Lost Time!

The mushroom mountains bloom with renewed life.
The waterfalls flow clear and pure once more.
The lost souls have found peace.

The island remembers your heroism.
The legend of your adventure will be told for ages to come.

Thank you for playing!
=================================================");
    }
    
    public void ShowHint()
    {
        string[] hints = new string[]
        {
            "Mushroom mountains can heal you when you're near them.",
            "Use your horse to travel faster across the island.",
            "Rally your squad with H for help in tough battles.",
            "Waterfalls provide safe zones - enemies avoid them.",
            "Different enemy types have different attack patterns.",
            "Explore thoroughly to find all the mushroom mountains.",
            "The island holds many secrets - keep exploring!"
        };
        
        Debug.Log($"HINT: {hints[Random.Range(0, hints.Length)]}");
    }
    
    void Update()
    {
        // Show hint with F1
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ShowHint();
        }
        
        // Show story progress with Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ShowProgress();
        }
    }
    
    void ShowProgress()
    {
        Debug.Log($@"
=== STORY PROGRESS ===
Current Chapter: {currentChapter} / {totalChapters}
Enemies Defeated: {enemiesDefeated}
Enemies Needed for Next Chapter: {GetEnemiesRequiredForCurrentChapter()}
{GetCurrentChapterText()}
=====================");
    }
}
