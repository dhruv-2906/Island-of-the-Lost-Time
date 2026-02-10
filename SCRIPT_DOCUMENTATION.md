# Script Documentation - Island of the Lost Time

Complete reference for all game scripts and their functionality.

## 🎮 Player Systems

### PlayerController.cs
**Purpose:** Main player character controller

**Public Fields:**
- `walkSpeed` (float): Walking speed (default: 4)
- `gallopSpeed` (float): Running/galloping speed (default: 8)
- `gravity` (float): Gravity force (default: -9.81)
- `mountSocket` (Transform): Position for mounting
- `isMounted` (bool, readonly): Whether player is on horse
- `horse` (HorseController): Reference to horse

**Controls:**
- WASD: Movement
- E: Mount/dismount horse
- Shift: Sprint/gallop
- Left-click: Attack
- H: Rally squad

**Dependencies:**
- CharacterController (required)
- MeleeAttack (optional, for combat)

---

### CameraOrbit.cs
**Purpose:** Third-person camera that orbits around player

**Public Fields:**
- `target` (Transform): Player to follow
- `distance` (float): Distance from player
- `xSpeed` (float): Horizontal rotation speed
- `ySpeed` (float): Vertical rotation speed
- `yMinLimit` (float): Min vertical angle
- `yMaxLimit` (float): Max vertical angle
- `minDistance` (float): Min zoom distance
- `maxDistance` (float): Max zoom distance

**Controls:**
- Right-click + drag: Rotate camera
- Scroll wheel: Zoom in/out

---

### HorseController.cs
**Purpose:** Horse mount system

**Public Fields:**
- `walkSpeed` (float): Horse walking speed
- `gallopSpeed` (float): Horse galloping speed
- `mountPoint` (Transform): Where player sits

**Dependencies:**
- Rigidbody (required)

**Usage:**
- Automatically controlled when player mounts
- Player can attack while mounted

---

## ⚔️ Combat Systems

### MeleeAttack.cs
**Purpose:** Sphere-based melee attack system

**Public Fields:**
- `range` (float): Attack range radius
- `damage` (int): Damage per attack
- `hitMask` (LayerMask): Layers that can be hit
- `hitVFX` (GameObject): Particle effect on hit

**Methods:**
- `Attack()`: Perform attack (called by PlayerController)

**How it works:**
1. Creates sphere at player position
2. Detects all colliders in range
3. Damages objects with Health component
4. Spawns VFX at hit location

---

### Health.cs
**Purpose:** Universal health system for player and enemies

**Public Fields:**
- `maxHP` (int): Maximum health
- `currentHP` (int): Current health

**Methods:**
- `TakeDamage(int amount)`: Reduce health
- `Heal(int amount)`: Restore health
- `Die()`: Called when health reaches 0

**Features:**
- Automatically notifies GameManager when enemies die
- Destroys GameObject on death

---

### EnemyAI.cs
**Purpose:** Advanced AI for enemy behavior

**Public Fields:**
- `enemyType` (EnemyType): Type of enemy
- `detectionRange` (float): How far enemy sees
- `attackRange` (float): Attack distance
- `attackDamage` (int): Damage per attack
- `attackCooldown` (float): Time between attacks
- `patrolPoints` (Transform[]): Waypoints
- `patrolWaitTime` (float): Wait time at waypoints
- `returnToPatrol` (bool): Return after losing player
- `callForHelp` (bool): Alert nearby enemies
- `helpCallRadius` (float): Help call range

**Enemy Types:**
- MushroomGuardian: Guards mushroom mountains
- WaterfallSentinel: Guards waterfalls
- LostSoul: Aggressive wanderer
- ForestWarden: Patrol-focused

**AI States:**
- Idle: Standing still
- Patrol: Following waypoints
- Chase: Pursuing player
- Attack: Attacking player
- Retreat: Moving away
- Dead: Defeated

**Dependencies:**
- NavMeshAgent (required)
- Health (required)

---

### SquadAI.cs
**Purpose:** AI companions that follow player

**Public Fields:**
- `leader` (Transform): Who to follow
- `followDistance` (float): Distance behind leader

**Methods:**
- `SetLeader(Transform)`: Set new leader
- `Rally(Transform)`: Called by horn

**Dependencies:**
- NavMeshAgent (required)

---

## 🌍 Environment Systems

### MushroomMountain.cs
**Purpose:** Procedural mushroom mountain generation

**Public Fields:**
- `height` (float): Total height
- `capRadius` (float): Cap size
- `stemRadius` (float): Stem thickness
- `capColor` (Color): Cap color
- `stemColor` (Color): Stem color
- `hasSpots` (bool): Add spots
- `spotCount` (int): Number of spots
- `isClimbable` (bool): Can climb
- `healingAmount` (int): HP restored
- `healingRadius` (float): Healing range

**Features:**
- Automatically generates on Start()
- Heals nearby players over time
- Procedurally creates cap and stem
- Randomized spot placement

---

### WaterfallEffect.cs
**Purpose:** Realistic waterfall with particles

**Public Fields:**
- `waterfallHeight` (float): Drop height
- `waterfallWidth` (float): Width
- `flowRate` (float): Particles per second
- `waterColor` (Color): Water color
- `createMist` (bool): Add mist effect
- `createSplash` (bool): Splash on entry
- `waterfallSound` (AudioClip): Sound effect

**Features:**
- Particle-based water flow
- Mist effect at bottom
- Water pool with trigger
- Splash effects on entry

**Dependencies:**
- ParticleSystem (required)
- AudioSource (optional)

---

### EnvironmentManager.cs
**Purpose:** Manages world generation

**Public Fields:**
- `autoGenerate` (bool): Generate on start
- `mushroomMountainCount` (int): Number of mushrooms
- `waterfallCount` (int): Number of waterfalls
- `generationRadius` (float): Spawn area size
- `mushroomMountainPrefab` (GameObject): Custom prefab
- `waterfallPrefab` (GameObject): Custom prefab
- `enableFog` (bool): Enable fog
- `fogColor` (Color): Fog color
- `fogDensity` (float): Fog thickness
- `ambientColor` (Color): Ambient light

**Methods:**
- `GenerateEnvironment()`: Create all elements
- `RegenerateEnvironment()`: Clear and regenerate

**Features:**
- Procedural placement
- Randomized properties
- Atmospheric effects
- Fog and lighting setup

---

## 🎭 Game Management

### GameManager.cs
**Purpose:** Central game controller

**Public Fields:**
- `squadMembers` (Transform[]): Squad members
- `storyManager` (StoryManager): Story system
- `environmentManager` (EnvironmentManager): Environment

**Methods:**
- `RallySquad(Vector3)`: Command squad
- `OnEnemyDefeated(GameObject)`: Track kills

**Features:**
- Singleton pattern
- Auto-finds systems
- Coordinates game events

---

### StoryManager.cs
**Purpose:** Story progression and narrative

**Public Fields:**
- `currentChapter` (int): Current chapter
- `totalChapters` (int): Total chapters
- `introText` (string): Introduction
- `chapterTitles` (string[]): Chapter names
- `chapterDescriptions` (string[]): Chapter descriptions

**Methods:**
- `OnEnemyDefeated(GameObject)`: Track progress
- `GetCurrentChapterText()`: Get chapter info
- `ShowHint()`: Display random hint
- `ShowProgress()`: Show current progress

**Features:**
- 5-chapter campaign
- Automatic progression
- Hints system (F1)
- Progress display (Tab)

---

### GameUI.cs
**Purpose:** All UI rendering

**Public Fields:**
- `player` (PlayerController): Player reference
- `storyManager` (StoryManager): Story reference
- `showDebugInfo` (bool): Show debug overlay
- `showControls` (bool): Show control list
- `fontSize` (int): Text size

**Features:**
- Health bar with color coding
- Controls display
- Story progress
- Debug information
- FPS counter
- Enemy counter

---

### SceneSetup.cs
**Purpose:** Automated scene initialization

**Public Fields:**
- `autoSetup` (bool): Setup on start
- `createPlayer` (bool): Create player
- `createEnemies` (bool): Create enemies
- `enemyCount` (int): Number of enemies
- `createCamera` (bool): Setup camera
- `createGround` (bool): Create ground
- `setupLighting` (bool): Setup lights

**Methods:**
- `SetupScene()`: Create everything
- `CreateNavMeshInfo()`: NavMesh instructions

**Features:**
- One-click scene setup
- Configurable generation
- Proper component setup
- Automatic tagging

---

## 🔧 Usage Examples

### Create a Custom Enemy Type
```csharp
GameObject enemy = new GameObject("CustomEnemy");
enemy.tag = "Enemy";

NavMeshAgent agent = enemy.AddComponent<NavMeshAgent>();
EnemyAI ai = enemy.AddComponent<EnemyAI>();
ai.enemyType = EnemyAI.EnemyType.MushroomGuardian;
ai.detectionRange = 20f;
ai.attackDamage = 30;

Health health = enemy.AddComponent<Health>();
health.maxHP = 150;
```

### Spawn Mushroom Mountain at Position
```csharp
GameObject mushroom = new GameObject("MyMushroom");
mushroom.transform.position = new Vector3(10, 0, 10);

MushroomMountain script = mushroom.AddComponent<MushroomMountain>();
script.height = 25f;
script.capColor = Color.blue;
script.healingAmount = 20;
```

### Create Custom Chapter
```csharp
StoryManager story = FindObjectOfType<StoryManager>();
story.chapterTitles[0] = "My Custom Chapter";
story.chapterDescriptions[0] = "Your custom description";
```

---

## 🎯 Best Practices

1. **Always bake NavMesh** after adding ground
2. **Tag objects properly** (Player, Enemy)
3. **Set layer masks** for MeleeAttack
4. **Use prefabs** for reusable objects
5. **Test in Editor** before building
6. **Optimize particle counts** for WebGL
7. **Keep draw calls low** for performance

---

## 📊 Performance Tips

- Use object pooling for enemies
- Limit particle emission rates
- Reduce mesh complexity
- Use LOD for distant objects
- Bake lighting when possible
- Optimize collision layers
- Profile with Unity Profiler

---

For more information, see the main README.md file.
