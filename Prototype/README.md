# Island of the Lost Time — 3D Adventure Game Prototype

This repository contains a Unity WebGL prototype for a third-person adventure game featuring mushroom mountains, waterfalls, and epic combat.

## 🎮 Game Features

### Environment
- **Mushroom Mountains**: Procedurally generated giant mushrooms that heal nearby players
- **Waterfalls**: Dynamic particle-based waterfalls with mist and splash effects
- **Atmospheric Effects**: Fog, ambient lighting, and immersive 3D environments

### Gameplay
- **Third-Person Controller**: WASD movement with mouse orbit camera
- **Mount System**: Mount/dismount horse (press E), Shift to gallop
- **Combat System**: Melee attack (left-click) with overlap-sphere damage and VFX
- **Squad System**: Rally AI companions with horn (press H) - squad follows using NavMesh
- **Story Campaign**: 5-chapter adventure with progressive difficulty
- **Enemy AI**: Advanced AI with patrol, chase, attack, and coordination behaviors

### Story
Progress through 5 epic chapters:
1. Awakening in the Mushroom Forest
2. The Waterfall Sentinels
3. The Lost Souls
4. The Corruption Source
5. The Final Guardian

## 🚀 Quick Start

### Option 1: Automated Setup (Recommended)
1. Open Unity Hub and add the project folder: `Island-of-the-Lost-Time/Prototype`
2. Recommended Unity: **2024.3 LTS** (URP recommended for WebGL)
3. Open `Assets/Scenes/Main.unity`
4. Add an empty GameObject to the scene and attach the `SceneSetup` script
5. The scene will automatically create:
   - Player with controls and combat
   - Camera with orbit system
   - Ground terrain
   - Enemies with AI
   - Environment (mushroom mountains and waterfalls)
   - All game managers

### Option 2: Manual Setup
1. Open Unity Hub and add project folder
2. Install WebGL build support if needed
3. Open `Assets/Scenes/Main.unity`
4. Manually add game objects using the scripts in `Assets/Scripts/`

### Controls
- **WASD** - Move character
- **Mouse** - Look around
- **E** - Mount/Dismount horse
- **Shift** - Sprint/Gallop
- **Left Click** - Melee attack
- **H** - Rally squad
- **Tab** - Show story progress
- **F1** - Show gameplay hint

## 📁 Project Structure

```
Prototype/
├── Assets/
│   ├── Scripts/
│   │   ├── PlayerController.cs      # Third-person player movement
│   │   ├── CameraOrbit.cs          # Mouse-based camera control
│   │   ├── HorseController.cs      # Mount/dismount system
│   │   ├── MeleeAttack.cs          # Combat system
│   │   ├── Health.cs               # Health management
│   │   ├── EnemyAI.cs              # Advanced enemy AI
│   │   ├── SquadAI.cs              # Companion AI
│   │   ├── MushroomMountain.cs     # Procedural mushroom generation
│   │   ├── WaterfallEffect.cs      # Waterfall particle effects
│   │   ├── EnvironmentManager.cs   # Environment generation
│   │   ├── StoryManager.cs         # Story progression
│   │   ├── GameManager.cs          # Central game management
│   │   ├── GameUI.cs               # UI system
│   │   └── SceneSetup.cs           # Automated scene setup
│   └── Scenes/
│       └── Main.unity              # Main game scene
├── .github/
│   └── workflows/
│       └── unity-build.yml         # CI/CD for WebGL builds
└── README.md
```

## 🎨 Adding Custom Content

### Add Mushroom Mountains
```csharp
GameObject mushroom = new GameObject("Mushroom");
MushroomMountain script = mushroom.AddComponent<MushroomMountain>();
script.height = 20f;
script.capRadius = 15f;
script.capColor = new Color(0.8f, 0.2f, 0.2f);
```

### Add Waterfalls
```csharp
GameObject waterfall = new GameObject("Waterfall");
WaterfallEffect script = waterfall.AddComponent<WaterfallEffect>();
script.waterfallHeight = 15f;
script.waterfallWidth = 5f;
script.createMist = true;
```

### Add Enemies
```csharp
GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
enemy.AddComponent<NavMeshAgent>();
enemy.AddComponent<EnemyAI>();
enemy.AddComponent<Health>();
enemy.tag = "Enemy";
```

## 🌐 WebGL Deployment

The repository includes GitHub Actions for automatic WebGL builds and deployment to GitHub Pages:

### Automatic Deployment
1. Push to the `main` branch
2. GitHub Actions builds WebGL version
3. Deploys to GitHub Pages automatically
4. Play online at your GitHub Pages URL

### Manual Build
1. File → Build Settings
2. Select WebGL platform
3. Click "Switch Platform"
4. Click "Build And Run"
5. Choose output folder

## 🛠️ Development

### Prerequisites
- Unity 2024.3 LTS or newer
- WebGL Build Support module
- Git for version control

### Setting Up NavMesh (Required for Enemy AI)
1. Window → AI → Navigation
2. Select ground plane
3. Check "Navigation Static"
4. Click "Bake" button
5. Adjust agent settings:
   - Agent Radius: 0.5
   - Agent Height: 2
   - Max Slope: 45

### Testing
1. Press Play in Unity Editor
2. Use WASD to move around
3. Test mounting/dismounting with E
4. Test combat with Left Click
5. Test squad rally with H

## 🎯 Game Systems

### Enemy Types
- **Mushroom Guardian**: Guards mushroom mountains
- **Waterfall Sentinel**: Protects waterfalls
- **Lost Soul**: Aggressive wandering enemy
- **Forest Warden**: Patrol-focused protector

### AI Behaviors
- **Patrol**: Enemies patrol between waypoints
- **Detection**: Enemies detect player within range
- **Chase**: Enemies pursue detected players
- **Attack**: Enemies attack when in range
- **Call for Help**: Enemies alert nearby allies

### Healing System
- Stand near mushroom mountains to heal over time
- Health regenerates slowly in safe zones
- Monitor health bar in top-left corner

## 📊 Technical Details

### Performance
- Optimized for WebGL deployment
- Particle systems use efficient emission rates
- LOD system recommended for production builds

### Rendering
- Standard shader pipeline
- Supports URP for better WebGL performance
- Fog and atmospheric effects for immersion

## 🐛 Troubleshooting

**Enemies don't move:**
- Ensure NavMesh is baked (Window → AI → Navigation)
- Check ground is marked as Navigation Static

**Player falls through ground:**
- Verify ground has a collider
- Check CharacterController settings

**Camera doesn't follow:**
- Ensure CameraOrbit script has target assigned
- Check camera is tagged "MainCamera"

## 📝 License

MIT License — see `LICENSE` file for details

## 🎮 Next Steps

To expand this into a full online multiplayer game:
1. Add Unity Netcode or Photon PUN 2
2. Implement player synchronization
3. Add matchmaking and lobbies
4. Create cloud save system
5. Add more character models and animations
6. Expand story with more chapters
7. Add inventory and equipment systems
8. Create boss battles and special encounters

---

**Enjoy your adventure on the Island of the Lost Time!** 🍄💧⚔️
