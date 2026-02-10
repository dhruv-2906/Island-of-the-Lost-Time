# Quick Setup Guide - Island of the Lost Time

This guide will help you quickly set up and start playing the 3D adventure game.

## 🚀 5-Minute Setup

### Step 1: Open in Unity
1. Install Unity Hub from https://unity.com/download
2. Install Unity 2024.3 LTS (or newer)
3. In Unity Hub, click "Add" and select the `/Prototype` folder
4. Open the project

### Step 2: Automated Scene Setup
1. In Unity, go to `File → New Scene`
2. Create an empty GameObject: `GameObject → Create Empty`
3. Name it "SceneSetup"
4. In Inspector, click "Add Component"
5. Search for "SceneSetup" and add it
6. The scene will automatically create everything you need!

### Step 3: Setup NavMesh (Important!)
For enemies to work properly:
1. Go to `Window → AI → Navigation`
2. Select the "Ground" object in the hierarchy
3. In the Navigation window, check "Navigation Static"
4. Click the "Bake" button at the bottom
5. Wait for baking to complete (a few seconds)

### Step 4: Play!
1. Press the Play button (▶️) at the top
2. Use WASD to move
3. Mouse to look around
4. Left-click to attack
5. Enjoy the adventure!

## 🎮 Alternative: Manual Setup

If you prefer manual control:

### Create Player
1. `GameObject → 3D Object → Capsule`
2. Name it "Player", tag it as "Player"
3. Add components:
   - Character Controller
   - Player Controller (script)
   - Health (script)
   - Melee Attack (script)

### Create Camera
1. Select Main Camera
2. Add CameraOrbit script
3. Drag Player to the "Target" field

### Create Ground
1. `GameObject → 3D Object → Plane`
2. Scale it up: X=20, Y=1, Z=20

### Create Environment
1. `GameObject → Create Empty`
2. Name it "EnvironmentManager"
3. Add EnvironmentManager script
4. It will create mushroom mountains and waterfalls

### Create Enemies
1. `GameObject → 3D Object → Capsule`
2. Tag as "Enemy"
3. Add components:
   - Nav Mesh Agent
   - Enemy AI (script)
   - Health (script)

### Create Managers
1. Create empty objects for:
   - GameManager
   - StoryManager
   - GameUI

## 🎯 What You Get

After setup, your scene includes:

✅ **Player Character**
- Full movement controls (WASD)
- Combat system (Left-click)
- Health system

✅ **Camera System**
- Smooth follow camera
- Mouse orbit control
- Zoom with scroll wheel

✅ **Environment**
- 5 procedural mushroom mountains
- 3 waterfalls with particle effects
- Atmospheric fog and lighting

✅ **Enemies**
- 5 AI enemies
- Patrol, chase, and attack behaviors
- Health system

✅ **Game Systems**
- Story progression (5 chapters)
- UI with health bar and controls
- Squad rally system

## ⚠️ Common Issues

### "Enemies don't move"
**Fix:** Bake the NavMesh (see Step 3 above)

### "Player falls through ground"
**Fix:** Make sure the ground has a Mesh Collider

### "Camera doesn't follow"
**Fix:** 
1. Select Main Camera
2. Find CameraOrbit component
3. Drag Player object to "Target" field

### "Scripts have errors"
**Fix:** 
1. Go to `Assets → Reimport All`
2. Wait for Unity to recompile
3. Check Console for specific errors

### "No mushroom mountains appear"
**Fix:**
1. Select EnvironmentManager object
2. Check "Auto Generate" is enabled
3. Press Play again

## 🎮 Testing Your Setup

Run through this checklist:
- [ ] Player can move with WASD
- [ ] Camera follows player
- [ ] Right-click rotates camera
- [ ] Scroll wheel zooms
- [ ] Left-click attacks
- [ ] Health bar shows in top-left
- [ ] Mushroom mountains are visible
- [ ] Waterfalls are flowing
- [ ] Enemies are moving around
- [ ] Tab shows story progress
- [ ] F1 shows hints

If all checked, you're ready to play! 🎉

## 🌐 Build for Web

To make it playable online:

1. `File → Build Settings`
2. Select "WebGL" platform
3. Click "Switch Platform"
4. Click "Build"
5. Choose output folder
6. Wait for build (5-15 minutes)
7. Upload to itch.io, GitHub Pages, or your web server

## 📚 Next Steps

1. **Customize Mushroom Mountains:**
   - Select a MushroomMountain object
   - Adjust colors, size, healing in Inspector

2. **Add More Enemies:**
   - Duplicate existing enemy
   - Change position and patrol points

3. **Modify Story:**
   - Select StoryManager
   - Edit chapter titles and descriptions

4. **Change Environment:**
   - Select EnvironmentManager
   - Adjust fog, lighting, generation counts

## 🎯 Pro Tips

- Press Tab in-game to see story progress
- Press F1 for gameplay hints
- Mushroom mountains heal you when nearby
- Rally squad with H for help in battles
- Explore to find all mushroom mountains
- Different colored mushrooms have different properties

## 🆘 Need Help?

Check the main README.md for:
- Detailed documentation
- Script explanations
- Advanced customization
- Troubleshooting guide

---

**Happy Gaming! Enjoy the Island of the Lost Time!** 🍄✨
